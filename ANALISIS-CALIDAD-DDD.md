# Análisis de calidad — DDD y arquitectura hexagonal

> Backend `LuxuryBiker.Backend`. Análisis sobre el estado tras el PR #3.
> Fecha: 2026-09-04. Los puntos marcados ✅ se corrigieron en la rama
> `refactor/ddd-hexagonal`; los marcados 📋 quedan documentados como deuda.

## Cómo se evaluó

Dos criterios, aplicados por separado:

- **Arquitectura hexagonal**: la dirección de las dependencias. El núcleo (Domain)
  no debe conocer a nadie; Application define *puertos*; Infrastructure y Api son
  *adaptadores*. Se verificó con las `PackageReference` de cada `.csproj` y con los
  `using` reales entre capas.
- **DDD táctico**: si el modelo es rico o anémico, si los agregados protegen sus
  invariantes y si las reglas de negocio viven en el dominio.

---

## 1. Violaciones de arquitectura hexagonal

### 1.1 ✅ `Application` dependía de Entity Framework Core — **CRÍTICO**

`LuxuryBiker.Application.csproj` referenciaba `Microsoft.EntityFrameworkCore`, y
`GetDashboardQueryHandler` consultaba `DbSet<T>` directamente a través de
`ILuxuryBikerDbContext`. La capa de casos de uso quedaba atada a la tecnología de
persistencia: cambiar de ORM habría obligado a reescribir Application.

**Corregido**: se introdujo el puerto `IDashboardRepository` (en Domain), con un
`DashboardSnapshot` como modelo de lectura. La consulta agregada se movió al adaptador
`Infrastructure/Persistence/Repositories/Reporting/DashboardRepository.cs`. Se eliminó
`ILuxuryBikerDbContext` y **la referencia a EF Core desapareció de Application**.

El handler ahora solo da formato (rellenar 12 meses, 15 días, nombres de mes), que sí es
responsabilidad de la capa de aplicación.

### 1.2 ✅ `Api` dependía de tipos de `Infrastructure` — **ALTO**

`AuthenticationController` y `Api/Authentication/AutoMapping.cs` importaban
`LuxuryBiker.Infrastructure.Services.Authentication` para usar `ApplicationUserDTO`.
La presentación saltaba por encima de Application y se acoplaba a un adaptador.

Era además la causa de un olor visible: `IAuthenticationService<TUser>` estaba
*generificado sin motivo funcional*, solo para que el puerto no tuviera que nombrar un
DTO que vivía en el lugar equivocado.

**Corregido**: el DTO subió a `Application/Common/Models/AuthenticatedUserDto.cs`, la
interfaz pasó a ser `IAuthenticationService` (no genérica) y el perfil de AutoMapper se
movió junto al servicio que lo usa, en Infrastructure. El controlador ya no menciona
Infrastructure.

### 1.3 ✅ `Domain` dependía de MediatR — **MEDIO**

`Domain/GlobalUsings.cs` tenía `global using MediatR;` porque `BaseEvent : INotification`.
El núcleo del dominio dependía de un framework de mensajería de la capa de aplicación.

Agravante: **la maquinaria de eventos de dominio estaba muerta**. `BaseEntity` exponía
`AddDomainEvent`/`DomainEvents` y `LuxuryBikerDbContext.SaveChangesAsync` recorría y
publicaba eventos, pero **ninguna entidad llamaba nunca a `AddDomainEvent`**.

**Corregido**: se eliminó el andamiaje (`BaseEvent`, la colección de eventos en
`BaseEntity`, el bucle de publicación y la dependencia `IPublisher` del `DbContext`) y
con él la referencia a MediatR en Domain. MediatR se declaró explícitamente en
Application, que es donde se usa de verdad.

> Si más adelante se quieren eventos de dominio, la forma correcta es un `BaseEvent`
> puro (sin `INotification`) y un puerto `IDomainEventDispatcher` implementado en
> Infrastructure sobre MediatR.

### 1.4 📋 `Domain` depende de ASP.NET Identity — **ALTO, no corregido**

`ApplicationUser : IdentityUser` obliga a `LuxuryBiker.Domain.csproj` a referenciar
`Microsoft.AspNetCore.Identity.EntityFrameworkCore`. Es la violación hexagonal más
profunda que queda: una entidad del núcleo hereda de un tipo de infraestructura.

**Por qué no se tocó**: separar `ApplicationUser` (dominio) de la entidad de Identity
implica un modelo de usuario propio, ajustar `IdentityDbContext`, el `IdentityService` y
**una migración de esquema**. Es un cambio con riesgo real que merece su propia rama y su
propia validación contra datos existentes.

**Camino sugerido**: dejar la identidad como detalle de Infrastructure
(`ApplicationIdentityUser : IdentityUser`) y que el dominio tenga su propio `User`
referenciado solo por `UserId` (que es lo único que `Purchase`/`Sale` usan hoy).

### 1.5 📋 Los puertos de repositorio filtran semántica del adaptador — **BAJO**

Dos ejemplos:

- `IProductsRepository.GetByIdsAsync` documenta *"devuelve los productos rastreados por
  el contexto"*. El *change tracking* es un concepto de EF, no del dominio. El puerto
  está describiendo cómo se comporta su implementación.
- `GetPagedAsync(...)` devuelve `(IReadOnlyList<T> Items, int TotalCount)` y recibe
  `pageNumber`/`pageSize`: paginación es una preocupación de consulta, razonable en un
  puerto de lectura, pero convendría un tipo `Page<T>` propio del dominio en lugar de una
  tupla.

Funciona y es pragmático, pero conviene saber que la abstracción no es hermética: hoy el
contrato asume una unidad de trabajo estilo EF.

---

## 2. Problemas de DDD

### 2.1 ✅ Agregados anémicos: `Purchase` y `Sale` — **ALTO**

Ambas raíces exponían **todas** sus propiedades con `{ get; set; }` público, incluidos
`Total` y `Status`, y `Details` era `IEnumerable<T>` con setter público. Consecuencias:

- Cualquier código podía escribir `purchase.Total = 0` y romper la coherencia con el
  detalle: **la invariante central del agregado no estaba protegida**.
- Se podía reemplazar la colección de líneas entera desde fuera.
- La creación era en dos pasos (`new Purchase(...)` y después `SetDetails(...)`), así que
  existía una ventana en la que el agregado estaba a medio construir y con `Total = 0`.

**Corregido**: `Purchase` y `Sale` pasaron a tener setters privados, una lista privada
`_details` expuesta como `IReadOnlyCollection<T>`, constructor privado para EF y una
**fábrica `Register(...)`** que exige al menos una línea, un código no vacío y calcula el
total en el mismo acto de creación. `Total` ya no se puede fijar desde fuera.

### 2.2 ✅ Entidades hijas sin invariantes — **MEDIO**

`PurchaseDetail` y `SaleDetail` eran DTOs con setters públicos: nada impedía crear una
línea con `Quantity = -5` o `ProductValue` negativo. La validación existía solo en el
`FluentValidation` del comando, es decir, **fuera** del modelo.

**Corregido**: constructor `(productId, productValue, quantity)` que valida cantidad > 0,
valor ≥ 0 y producto obligatorio; setters privados; y una propiedad calculada `Subtotal`
que sustituye a la multiplicación repetida en varios sitios. La validación del comando se
mantiene: sirve para devolver un 400 legible, mientras el dominio garantiza el invariante.

### 2.3 ✅ `Product` podía existir en estado inválido — **MEDIO**

El alta hacía `_mapper.Map<Product>(dto)` y **después** `entity.SetInternalCode()`. Entre
ambas líneas el producto existía sin código, que es un dato obligatorio en base de datos
(`Code` es `IsRequired()`). Además `SetInternalCode()` era público, así que el código
interno podía regenerarse desde cualquier parte.

**Corregido**: fábrica `Product.Create(name, reference, description, status)` que calcula
el código dentro; `SetInternalCode()` pasó a privado; el mapeo `CreateProductDto → Product`
de AutoMapper se eliminó (construir un agregado por reflexión salta sus reglas).

De paso se corrigió un **bug latente**: `word.Substring(0, 2)` lanzaba
`ArgumentOutOfRangeException` con nombres que tuvieran una palabra de una sola letra
(p. ej. "A Casco"). Ahora se maneja el caso.

### 2.4 ✅ Regla de negocio en la capa de aplicación — **MEDIO**

Dos casos:

- **Numeración de documentos** (`CLB{n+1}`, `VLB{n+1}`): estaba en
  `Application/Common/CodeGenerator.cs`. Numerar facturas es una regla del negocio.
  **Corregido**: pasó a `Domain/Common/DocumentNumber.cs` con los prefijos como
  constantes del dominio.
- **Disponibilidad de stock**: el handler comparaba `requested > (product.Stock ?? 0)`,
  reimplementando una regla del producto. **Corregido**: se añadió
  `Product.HasStockFor(quantity)` y el handler ahora pregunta al agregado.

### 2.5 📋 `ThirdTypes.Provider = 1` acoplado al orden del *seed* — **MEDIO, no corregido**

`Domain/Constants/ThirdTypes.cs` fija los ids 1 y 2 asumiendo el orden en que
`LuxuryBikerDbContextInitialiser` inserta los tipos de tercero. Si alguien reordena el
*seed* o siembra en otro entorno, los combos de compras y ventas se cruzan en silencio.

**Sugerencia**: sembrar con ids explícitos (`HasData` con `Id` fijo) o buscar por nombre.

### 2.6 📋 El agregado no impide stock negativo — **BAJO, decisión de negocio**

`Product.DecreaseStock` resta sin comprobar disponibilidad; quien protege hoy es el
handler de venta. Se dejó así a propósito: `ChangePurchaseStatus` **necesita** poder dejar
el stock negativo al revertir una compra ya vendida. Si se quiere blindar, hay que
modelar antes qué debe pasar en ese escenario.

---

## 3. Lo que ya estaba bien

Para no dar una imagen sesgada, el diseño de partida acierta en varias cosas:

- **Separación en cuatro proyectos** con las referencias en el sentido correcto
  (`Api → Application → Domain`, `Infrastructure → Application`), y un único punto de
  composición en `Program.cs`.
- **CQRS con MediatR** y *behaviors* de pipeline para autorización y validación: las
  preocupaciones transversales están fuera de los handlers.
- **Puertos de repositorio declarados en Domain** e implementados en Infrastructure: la
  dirección de la dependencia es la correcta.
- **Catálogo de errores por agregado** (`ProductsErrors`, `SalesErrors`, …) con `ErrorOr`:
  los fallos de negocio son valores de retorno, no excepciones.
- **Configuración de EF por entidad** (`*Metadata`) fuera de las entidades, así que el
  dominio no lleva atributos de persistencia.

---

## 4. Verificación del refactor

Todo lo marcado ✅ se validó:

| Comprobación | Resultado |
|---|---|
| `dotnet build` de la solución | 0 errores |
| `dotnet test` | 21/21 |
| `dotnet ef migrations has-pending-model-changes` | *No changes* — el refactor es **schema-neutral**, no requiere migración |
| Lectura de agregados existentes | compras y ventas se materializan con constructor privado y colección `_details` |
| `Product.Create` | alta de producto → `EL-GUPR-GP10` |
| `Purchase.Register` | compra 3 × 80 000 + IVA → `CLB2`, total 285 600; stock 2 → 5; valor 80 000 |
| `Product.HasStockFor` | vender 99 con stock 5 → `400 Sales.InsufficientStock` |
| `Sale.Register` | venta 2 × 250 000 → `VLB2`; stock 5 → 3 |
| `ChangeStatus` venta y compra | cancelar venta → stock 5; cancelar compra → stock 2 |
| `Dashboard/GetData` (puerto nuevo) | KPIs y series correctas, respetando `Status = true` |

Que EF no detecte cambios de modelo es el dato importante: **encapsular el dominio no
alteró el esquema**, así que el refactor es seguro de desplegar sobre la base existente.

---

## 5. Orden sugerido para lo pendiente

1. **1.4** — desacoplar `ApplicationUser` de ASP.NET Identity (rama propia, con migración).
2. **2.5** — fijar los ids de `TypeThird` en el *seed*.
3. **1.5** — introducir un `Page<T>` de dominio y sacar la semántica de *tracking* de los
   puertos.
4. **2.6** — decidir la política de stock negativo antes de blindar el agregado.
