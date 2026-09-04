# Contexto de la migración — LuxuryBiker (POS)

> Documento de referencia para retomar el proyecto. Última actualización: 2026-09-02 (r2).

**Estado:** Fases 1–6 completadas. Paridad funcional con el POS legado alcanzada
(auth, terceros, productos, compras, ventas, inventario/stock y dashboard), con menú por
rol, toasts, 15 tests unitarios de backend y CI. **Prueba end-to-end contra SQL Server
(Docker) superada** (2026-09-02): login, listados, alta de tercero/producto, compra con
IVA (300 000 → 357 000) que sube stock a 2 y fija el último valor de compra, venta que
baja stock a 1, `ChangeStatus` que lo reintegra a 2, y `Dashboard/GetData` con series
mensuales/diarias y nombres de mes en español. Añadido después: edición de productos y
terceros, validación de stock en venta y más tests (21/21). Pendientes: solo las mejoras
opcionales de §6.

## 1. Visión general

**Qué es:** un sistema de punto de venta (POS). Módulos del negocio: Terceros
(proveedores/clientes), Productos/Inventario, Compras, Ventas y un Dashboard con
indicadores y gráficas.

**Qué se está haciendo:** reescribir el proyecto legado (**.NET multicapa + React CRA**,
servido desde `wwwroot`) hacia:

| Capa | Legado (`ReactLegado/`) | Objetivo (`LuxuryBikerAngularNet/`) |
|---|---|---|
| Backend | .NET "6 capas" con SPs/triggers en SQL Server | **.NET Clean Architecture** (`LuxuryBiker.Backend`): Domain / Application / Infrastructure / Api, con MediatR (CQRS), EF Core, AutoMapper, ErrorOr, JWT + ASP.NET Identity |
| Frontend | React 18 (CRA) + jQuery + Bootstrap 5 + SweetAlert2 | **Angular 17** standalone (`LuxuryBiker.ClientApp`) con arquitectura hexagonal propia (`domain` / `data` / `presentation`), ngx-translate, RxJS |
| UI/tema | Plantilla SB-Admin-like | Plantilla **Melody** (Bootstrap admin), assets copiados a `presentation/assets/melody` |

> El **backend legado** está clonado en `c:\Proyectos\MigracionLuxuryBiker\LegadoNet\`
> (repo `github.com/chrisgzon/LuxuryBiker`, commit `726cc59`). Es la fuente de verdad
> para las reglas de negocio (IVA, stock, generación de códigos, dashboard). Resumen de
> lo extraído en la **sección 8**.

## 2. Arquitectura objetivo

### Backend — Clean Architecture (4 proyectos)
- **Domain**: entidades, value objects, eventos de dominio, constantes, interfaces de repositorio, tipos de error. Sin dependencias de infraestructura.
- **Application**: casos de uso como *commands/queries* de MediatR, DTOs, validación (FluentValidation), *behaviors* de pipeline (autorización, validación), interfaces de servicios.
- **Infrastructure**: EF Core (`LuxuryBikerDbContext`), configuración Fluent por entidad, migraciones, repositorios, servicios (JWT, Identity).
- **Api**: controllers finos que solo mapean `Model → DTO` y despachan por MediatR; Swagger, CORS, middleware de excepciones.

### Frontend — arquitectura hexagonal
- `src/base/` — abstracciones (`UseCase`, `Mapper`).
- `src/domain/` — modelos, interfaces de repositorio y *use cases* (sin Angular).
- `src/data/` — repositorios de implementación (HTTP), mappers `entity ↔ model`, *providers* de DI.
- `src/presentation/` — la app Angular (componentes, layouts Melody, servicios, guards, i18n).

## 3. Estado actual — lo que YA existe

### 3.1 Backend `LuxuryBiker.Backend`
- **Domain**: entidades `Product`, `Third`, `TypeThird`, `Purchase`, `PurchaseDetail`,
  `Sale`, `SaleDetail`, `ApplicationUser` (Identity). `BaseAuditableEntity` con eventos de
  dominio. `Roles` (`Administrator`, `Seller`), `Policies`. Interfaces
  `IProductsRepository`, `IThirdRepository` + genéricas. `Product.SetInternalCode()`.
- **Application**: MediatR + AutoMapper + ErrorOr. Comandos completos
  `CreateProductCommand` y `CreateThirdCommand` (handler, validación de duplicados,
  `[Authorize]`). `AuthorizationBehaviour`. Interfaces de servicios.
- **Infrastructure**: `LuxuryBikerDbContext : IdentityDbContext`. Metadata Fluent para
  todas las tablas (`T_PRODUCTS`, `T_THIRDS`, `T_TYPE_THIRD`, compras, ventas).
  3 migraciones. `LuxuryBikerDbContextInitialiser` (migra + *seed*: admin, roles, tipos
  de tercero). `AuthenticationService` (JWT), `IdentityService`. Repositorios de Products
  y Thirds con **solo `CreateAsync` + `GetBy…`** implementados.
- **Api**: `AuthenticationController` (`Login`, `GetProfileCurrentUser`),
  `ProductsController` (`Create`), `ThirdsController` (`Create`). Swagger con JWT, CORS
  `localhost:4200`, `GlobalExceptionHandlingMiddleware`.

### 3.2 Frontend `LuxuryBiker.ClientApp` (Angular 17)
- `domain` + `data` cableados para: **authentication** (login, get-profile), **products**
  (create), **thirds** (create), **users** (modelo).
- **Presentation**:
  - Ruteo: `login` + shell autenticado (`master`) con hijos `home`, `thirds/create`,
    `products/create`, `purchases/register`. Guards `isLoggedGuard` / `isntLoggedGuard`
    según presencia de JWT.
  - Layout Melody: `master`, `nav` (idioma es/en, logout), `sidebar`, `footer`,
    `settings`, `loader`.
  - Páginas funcionales end-to-end: **login**, **create-product**, **create-third**.
  - `AuthService` (login, logout, JWT en `localStorage`, `user$` / `jwt$`) +
    `AuthInterceptorHttpService` (Bearer, 401 → logout).
  - i18n ngx-translate (`es.json` / `en.json`). `ngx-currency` (COP).

### 3.3 Plantilla Melody
HTML estático en `templates/Melody/` — referencia de marcado para páginas, tablas y
gráficas.

## 4. Paridad funcional con el legado (checklist)

| Módulo | Funcionalidad legada | ¿En el nuevo? |
|---|---|---|
| Auth | Login JWT, logout, perfil | ✅ (el perfil no se recarga tras F5 — corregido en Fase 1) |
| Auth | Registro de usuario | ❌ |
| Terceros | Registrar cliente/distribuidor | ✅ |
| Terceros | Listado | ✅ (Fase 2, paginado + filtro por tipo) |
| Productos | Registrar (genera código único) | ✅ |
| Productos | Listado | ✅ (Fase 2, paginado) |
| Compras | Registrar compra: cabecera + N líneas, IVA 19% opcional, totales | ✅ (Fase 3) |
| Compras | Alta inline de proveedor/producto (modal) | ❌ (se puede añadir luego) |
| Compras | Listar compras | ✅ (Fase 3, paginado + filtro por fechas) |
| Compras | Combos productos + proveedores | ✅ (Fase 3, `Purchases/GetFormData`) |
| Ventas | Registrar venta (cabecera + líneas; cliente opcional) | ✅ (Fase 4) |
| Ventas | Alta inline de cliente (modal) | ❌ (se puede añadir luego) |
| Ventas | Listar ventas | ✅ (Fase 4, paginado + filtro por fechas) |
| Ventas | Cambiar estado (validar/cancelar → ajusta stock) | ✅ (Fase 4) |
| Ventas | Combos productos + clientes | ✅ (Fase 4, `Sales/GetFormData`) |
| Compras | Cambiar estado (validar/cancelar → ajusta stock) | ✅ (Fase 4) |
| Dashboard | KPIs (ventas hoy, compras/ventas del mes) | ✅ (Fase 5) |
| Dashboard | Gráfica compras vs ventas mensual | ✅ (Fase 5, SVG) |
| Dashboard | Gráfica ventas últimos 15 días | ✅ (Fase 5, SVG) |
| Dashboard | Tabla "productos más vendidos" | ✅ (Fase 5) |
| Inventario | Aumento de stock al registrar compra | ✅ (Fase 3) |
| Inventario | Descuento de stock al registrar venta | ✅ (Fase 4) |
| Inventario | Reajuste de stock al cambiar estado compra/venta | ✅ (Fase 4) |

## 5. Roadmap por fases

### Fase 1 — Fundamentos ✅
- [x] Subir backend de `net6.0` a **`net8.0`** (no hay runtime .NET 6 en la máquina) y
      alinear paquetes EF Core / ASP.NET a 8.x.
- [x] `docker-compose.yml` con SQL Server 2022 para desarrollo.
- [x] `appsettings` con cadena de conexión al SQL de Docker; base `appsettings.json`.
- [x] Corregir rol `Sealer` → `Seller`.
- [x] Eliminar el código de plantilla `WeatherForecast`.
- [x] Angular: cargar el perfil del usuario al arrancar si hay JWT.
- [x] Angular: quitar el uso de `jQuery`/`select2`/`datepicker` de `register-purchase`
      (dejaba la ruta rota) y usar controles nativos.
- [x] Renombrar `src/enviroments` → `src/environments` + `environment.prod.ts`.
- [x] `README.md` de la solución con pasos de arranque.

### Fase 2 — Lecturas ✅
- [x] **FluentValidation** (`12.1.1`) + `ValidationBehaviour` en el pipeline de MediatR
      (tras `AuthorizationBehaviour`); errores → `ValidationException` → el
      `GlobalExceptionHandlingMiddleware` responde `400` con `ValidationProblemDetails`
      (`{ errors: { campo: [msg] } }`, compatible con el manejo del cliente Angular).
- [x] Validators: `CreateProductCommandValidator`, `CreateThirdCommandValidator`,
      `GetProductsQueryValidator`, `GetThirdsQueryValidator`.
- [x] `PaginatedList<T>` en `Application/Common/Models`.
- [x] Query `GetProductsQuery` (paginada, `onlyActive`) + `ProductBriefDto` +
      `GET Products/GetAll?pageNumber&pageSize&onlyActive`.
- [x] Query `GetThirdsQuery` (paginada, filtro `typeId`) + `ThirdBriefDto` +
      `GET Thirds/GetAll?pageNumber&pageSize&typeId`.
- [x] Repositorios: `GetPagedAsync` en `IProductsRepository` / `IThirdRepository`;
      `GetAsync()` y `UpdateAsync()` implementados (ya no lanzan `NotImplementedException`).
- [x] Front: modelo compartido `PaginatedResult<T>`, use cases `GetProductsUseCase` /
      `GetThirdsUseCase`, repos e `index.ts` (providers), servicios `getAll()`.
- [x] Front: páginas `products/list` y `thirds/list` (tabla Melody + paginación;
      terceros con filtro por tipo); rutas nuevas (`/products` y `/thirds` redirigen a
      `list`); enlaces en el sidebar; claves i18n es/en.

Verificado: `dotnet build` de la solución y `ng build` (dev + prod) en verde. Falta
prueba end-to-end contra la BD (Docker Desktop no estaba levantado en esta sesión).

### Fase 3 — Compras ✅
- [x] Dominio: `Taxes.IvaRate` (19), `ThirdTypes` (Provider=1, Client=2),
      `Product.IncreaseStock/DecreaseStock/RegisterPurchaseValue` (métodos de dominio;
      `Stock`/`Value` siguen con setter privado), constructor + `SetDetails(...)` en
      `Purchase` (calcula `Total` = subtotal + IVA si aplica), `PurchasesErrors`,
      `IPurchasesRepository`, `IProductsRepository.GetByIdsAsync` (rastreado).
- [x] `CreatePurchaseCommand` (`[Authorize]` Administrator/Seller): valida líneas y
      proveedor, genera código `CLB{n+1}`, arma la compra, **aumenta stock y actualiza el
      último valor de compra de cada producto**, y persiste todo en un único
      `SaveChangesAsync` (atómico). Resultado `{ Id, Code, Total }`.
- [x] `GetPurchasesQuery` (paginada, filtro `dateFrom`/`dateTo`) + `PurchaseBriefDto`.
- [x] `GetPurchaseFormDataQuery` → `{ Products, Suppliers(tipo Provider) }` para los combos.
- [x] Validators FluentValidation para el comando y las dos queries.
- [x] `PurchasesRepository` + registro en DI. Sin migración nueva (las tablas
      `T_PURCHASES` / `T_PURCHASE_DETAILS` ya existían; `Product` solo ganó métodos).
- [x] `PurchasesController`: `POST Purchases/Create`, `GET Purchases/GetAll`,
      `GET Purchases/GetFormData` + `PurchaseModel` + AutoMapping.
- [x] Front: dominio/datos/servicio de `purchases` (modelos, 3 use cases, repo, `index.ts`).
- [x] Front: `register-purchase` **reescrito y funcional** — carga combos al iniciar,
      editor de líneas con tabla, subtotal + IVA (19%) + total en vivo, evita productos
      duplicados, envía `CreatePurchaseModel` y muestra el código generado.
- [x] Front: página `purchases/list` (tabla Melody + paginación); rutas (`/purchases`
      redirige a `list`); enlace en el sidebar; claves i18n es/en.

Verificado: `dotnet build` de la solución y `ng build` (dev + prod) en verde. Pendiente
prueba end-to-end contra la BD (Docker Desktop no levantado en la sesión).

**No cubierto en esta fase** (queda para Fase 4 o una 3.5): `Purchases/ChangeStatus`
(invertir estado y reajustar stock; en el legado lo hacía el trigger
`tr_updStockCompraChangeStatus`).

### Fase 4 — Ventas ✅
- [x] Dominio: `Sale` (constructor + `SetDetails` + `ToggleStatus`), `Purchase.ToggleStatus`,
      `SalesErrors`, `PurchasesErrors.NotFound`, `ISalesRepository`,
      `IPurchasesRepository` ampliado (`GetByIdWithDetailsAsync`, `SaveChangesAsync`).
- [x] `CodeGenerator` compartido (`CLB`/`VLB{n+1}`); `CreatePurchaseCommand` refactorizado
      para usarlo.
- [x] `CreateSaleCommand` (`[Authorize]` Administrator/Seller): valida líneas + cliente
      (opcional), código `VLB{n+1}`, fecha = ahora, **descuenta stock** por línea, persiste
      atómico. `GetSalesQuery` (paginado + filtro fechas) + `SaleBriefDto`.
      `GetSaleFormDataQuery` → `{ products, clients }`.
- [x] `ChangeSaleStatusCommand` y `ChangePurchaseStatusCommand`: invierten el estado y
      **reajustan el stock** (venta validada resta / cancelada reintegra; compra validada
      suma / cancelada resta) en una transacción — reemplazan a los triggers SQL del
      legado. Resultado `ChangeStatusResult { Id, Status }`.
- [x] `SalesRepository` + registro en DI. Sin migración nueva.
- [x] API: `SalesController` (`Create`, `ChangeStatus`, `GetAll`, `GetFormData`),
      `PurchasesController.ChangeStatus`, `ChangeStatusRequest`, `SaleModel` + AutoMapping.
- [x] Front: módulo `sales` completo (modelos, 4 use cases, repo, servicio); páginas
      `sales/register` (cliente opcional, "venta al público") y `sales/list` (con botón de
      toggle de estado + confirmación). `purchases/list` también gana el toggle de estado.
      Rutas, providers, sección **Ventas** en el sidebar, i18n es/en.

Verificado: `dotnet build` de la solución y `ng build` (dev + prod) en verde. Pendiente
prueba end-to-end contra la BD (Docker Desktop no levantado en la sesión).

### Fase 5 — Dashboard ✅
- [x] `GetDashboardQuery` (`[Authorize]`) → `DashboardDto`: consulta agregada única sobre
      `ILuxuryBikerDbContext` (solo `Status = true`). Devuelve: ventas de hoy, compras y
      ventas del mes, series mensuales de compras y ventas del año en curso (12 posiciones,
      rellenas con ceros), ventas de los últimos 15 días (15 posiciones), y top 10 de
      productos más vendidos del año (`join` explícito ventas–detalles–productos).
- [x] `DashboardController` → `GET Dashboard/GetData`. Sin migración nueva.
- [x] Front: módulo `dashboard` (modelo, use case, repo, servicio, providers).
- [x] `home.component` reescrito: 3 KPIs, gráfico de barras compras vs ventas (mensual) y
      gráfico de líneas de ventas diarias — **SVG inline, sin librería de charts** — más la
      tabla de productos más vendidos. Claves i18n `dashboard.*` es/en.

Verificado end-to-end contra SQL Server: las traducciones EF de
`DateTimeOffset.Year`/`.Month`, el `GroupBy` diario y el `join` de productos más vendidos
funcionan; los nombres de mes salen en español.

### Fase 6 — Transversal ✅
- [x] **Menú por rol**: `AuthService` expone `roles$`, `hasAnyRole(...)` y `profileResolved$`
      (para no rechazar por carrera de init). `roleGuard(...roles)` protege las rutas
      `thirds` / `products` / `purchases` / `sales` (rol `Administrator` o `Seller`); el
      sidebar oculta esos menús con `@if (canManage)`. `ROLES` en
      `user-logged.model.ts`. `UserLoggedModel.roles` corregido a `string[]` (lo que
      realmente envía el backend).
- [x] **Notificaciones/toasts**: `ToastService` + `ToastContainerComponent` propios (sin
      dependencia), montados en el `master` layout; cableados en alta de producto/tercero,
      registro de compra/venta y cambio de estado (éxito y error). Se conservan también
      las alertas inline.
- [x] **Pruebas**: nuevo proyecto `LuxuryBiker.Backend/tests/LuxuryBiker.Application.UnitTests`
      (xUnit + Moq + FluentAssertions) — **15 tests** sobre `CodeGenerator`, IVA/stock/errores
      de `CreatePurchase`/`CreateSale` y el reajuste de stock de los `ChangeStatus`. Los
      `*.spec.ts` del front que estaban rotos (import de un `export default` como *named*)
      se corrigieron y ampliaron.
- [x] **CI**: `.github/workflows/ci.yml` con dos jobs — backend (`dotnet restore/build/test`
      en Release) y frontend (`npm ci` + `npm run build`).

Verificado: `dotnet build` + `dotnet test` (15/15) y `ng build` (dev + prod) en verde.

### Mejoras adicionales (post-roadmap) ✅

- [x] **Edición de Productos y Terceros**: `UpdateProductCommand` / `UpdateThirdCommand`
      (+ validators, regeneran el código del producto al cambiar nombre/referencia,
      verifican unicidad excluyendo el propio registro) y queries `GetProductByIdQuery` /
      `GetThirdByIdQuery`. API: `POST Products/Update`, `GET Products/GetById` (ídem
      Thirds). Front: páginas `products/edit/:id` y `thirds/edit/:id` con botón *Editar*
      en los listados; use cases, repos, servicios y providers.
- [x] **Validación de stock en venta**: `CreateSaleCommand` rechaza la venta con
      `Sales.InsufficientStock` si la cantidad solicitada supera el stock disponible
      (a diferencia del legado, que permitía stock negativo).
- [x] Tests: 21/21 (nuevos para stock insuficiente y para los handlers de actualización).
- Verificado end-to-end contra SQL Server: `Update` regenera el código, `GetById`
  responde, y la venta de 10 con stock 2 devuelve 400 con el detalle del error.

## 6. Deudas técnicas / riesgos

- Generación de código `CLB{n+1}` / `VLB{n+1}` sin bloqueo: dos operaciones concurrentes
  podrían obtener el mismo código (mismo defecto que el legado). Aceptable para el volumen
  actual; si molesta, índice único + reintento.
- No hay alta inline de proveedor/producto/cliente desde los formularios de compra/venta
  (el legado abría un modal); se puede añadir más adelante.
- `Thirds/GetById` no incluye la navegación `Type`, así que `typeName` sale `null` (el
  formulario de edición usa `typeId`, que sí viene). Si se necesita el nombre, añadir el
  `Include`.
- Gráficas del dashboard en SVG hecho a mano (sin dependencia). Si se quiere interacción
  (tooltips ricos, zoom), migrar a `ng2-charts`/Chart.js.
- Nombres de mes del dashboard se calculan en el backend con `CultureInfo("es-ES")`; en un
  contenedor Linux requiere ICU (presente en las imágenes base de .NET).
- JWT `Secret` débil y versionado en `appsettings.Development.json` — mover a User Secrets
  / variables de entorno antes de cualquier despliegue.
- Cobertura de tests centrada en handlers de compras/ventas y de actualización. Falta
  cubrir el dashboard y añadir tests de integración de la API; el job de CI del frontend
  solo compila (no ejecuta `ng test`, que necesita Chrome headless).
- `AutoMapper 13.0.1` mantiene el aviso NuGet NU1903 (pendiente de decidir bump/reemplazo).
- El `roleGuard` distingue `Administrator`/`Seller`, pero ambos roles autorizan todas las
  operaciones en el backend, así que hoy el gating es sobre todo cosmético.
- Idioma del código: el nuevo usa términos en inglés (`Third`, `Purchase`); el legado, en
  español.

## 7. Cómo ejecutar

Ver `README.md` en esta misma carpeta.

## 8. Referencia: reglas de negocio del backend legado

Extraídas de `LegadoNet/LuxuryBiker.WebApplication/` (arquitectura de 6 capas:
`Data.Entities`, `Data.CustomTypes`, `Data.Interfaces`, `Data.Model`, `Data.Repositry`,
`Logic`, + `Web`). Los scripts SQL están en
`LegadoNet/.../LuxuryBiker.Data.Model/MetaData/*.txt` (`QUERYINITIAL`, `STOREPROCEDURE`,
`TRIGGERCOMPRAS`, `TRIGGERVENTAStxt`).

### 8.1 Impuesto (IVA)
- Tasa fija **19%**, constante en `ComprasLogic` / `VentasLogic`.
- Se aplica sobre el total completo de la compra/venta **solo si `AplicaIva == true`**.
- `Total` persistido = suma de líneas (`cantidad * ValorProducto`) **+ IVA**. No se guarda
  el desglose de impuesto ni el subtotal.

### 8.2 Generación de códigos
- **Compra**: `CLB{n+1}`, donde `n` = parte numérica del último `CodCompra` (por
  `IdCompra` desc; si no hay, `0`).
- **Venta**: `VLB{n+1}`, misma lógica sobre `CodVenta`.
- **Producto**: `"P"` + (primeras 2 letras de hasta las 2 primeras palabras del nombre, en
  mayúsculas) + `Referencia` en mayúsculas sin espacios. Ej.: "Bocinas" ref `KL168` →
  `PBOKL168`. (El backend nuevo hace una variante `EL-XXYY-REF` en `Product.SetInternalCode()`;
  decidir cuál conservar.)

### 8.3 Stock — **clave para la migración**
El backend nuevo no tiene NADA de esto; hay que implementarlo en la capa Application
(idealmente dentro de una transacción / unidad de trabajo).

- **Al registrar una compra** (`ComprasLogic.RegisterNewCompra`): tras insertar la compra
  se llama, en C#:
  - `UpdateValueProducts` → fija `Producto.ValorProducto` = último valor de compra de cada
    producto.
  - `UpdateStockProducts(+)` → **suma** `cantidad` al `Stock` de cada producto.
- **Al registrar una venta** (`VentasLogic.RegisterNewVenta`): `UpdateStockProducts(-)` →
  **resta** `Cantidad` del `Stock`. No toca `ValorProducto`.
- **Al cambiar el estado** de una compra o venta: el C# **solo invierte el booleano
  `Estado`**. El ajuste de stock lo hacían **triggers de SQL Server**:
  - `tr_updStockCompraChangeStatus` (AFTER UPDATE en `Compras`): si pasa a `Estado=1` suma
    `cantidad` al stock; si pasa a `0`, la resta.
  - `tr_updStockVentaChangeStatus` (AFTER UPDATE en `Ventas`): si pasa a `Estado=1` **resta**
    `cantidad` del stock; si pasa a `0`, la **suma** (reintegra).
- Solo cuentan para stock y dashboard los registros con `Estado = true` (validados).
- No hay control de stock negativo ni transacción atómica entre "insertar venta" y
  "descontar stock" (bug del legado a corregir).

### 8.4 Dashboard — `GET Compras/GetData` (`ComprasRepository.GetData`)
Devuelve un diccionario con estas claves (solo considera `Estado = true`):

| Clave | Contenido |
|---|---|
| `totalCompraMes` | `{ Mes, Total }` — compras del mes actual |
| `totalVentaMes` | `{ Mes, Total }` — ventas del mes actual |
| `comprasMes` | hasta 12 `{ IndexMes, Mes, Total }` agrupado por mes |
| `ventasMes` | ídem para ventas |
| `ventasDia` | hasta 15 `{ Dia, Total }` agrupado por día, últimos 15 días |
| `ventasHoy` | `{ Total }` — ventas de hoy |
| `productosMasVendidos` | top 10 `{ Codigo, Nombre, IdProducto, Stock, Cantidad }` por unidades vendidas en el año en curso |

Cada agregado devuelve un valor por defecto (Total 0, mes/día actual) cuando no hay filas.

### 8.5 Esquema de datos (legado) — diferencias con el backend nuevo
- Tablas: `Productos`, `Terceros`, `TiposTercero`, `Users`, `UsrRoles`,
  `UsrUsuario_UsrRol`, `Compras`, `ComprasDetails`, `Ventas`, `VentasDetails`.
- Precisiones decimales: `Stock` `decimal(10,3)`, `ValorProducto` / `Total`
  `decimal(28,6)`. El backend nuevo usa `decimal(10,2)` y `decimal(28,2)` → alinear.
- `Ventas.TerceroIdTercero` es **NULL-able** (venta sin cliente registrado);
  `Compras.TerceroIdTercero` es obligatorio.
- Identidad: el legado tiene tablas propias `Users` / `UsrRoles` (rol semilla
  `"Super Administrador"`, usuario `superadmin@luxurybiker.com` / `admin123456`) y un SP
  `REGISTER_NEW_USER` que crea el usuario y le asigna el rol 2. El backend nuevo usa
  ASP.NET Identity (`administrator@luxurybiker.com` / `Administrator1!`).
- Autenticación legada: JWT propio con clave embebida `1a2b3c4d5e6f7g8h9qwerty`, claims
  `NameIdentifier` + `Name`, expiración 2 h (o 2 días con *rememberme*). Contraseñas con
  `PasswordHasher<string>` de Identity (semilla, sobre `userName`).

### 8.6 Contrato de la API legada (rutas que consume `ReactLegado/`)
Todas bajo `[Authorize]` salvo indicación; respuesta `ResponseGeneric<T>`
(`{ Error, Mensaje, Result, ExtraData }`).

| Ruta | Verbo | Descripción |
|---|---|---|
| `Usuarios/Login` | POST (anon) | login → `User` con `Token` |
| `Usuarios/Whoami` | GET | usuario autenticado (por claim) |
| `Usuarios/register` | POST (anon) | alta de usuario (vía SP) |
| `Terceros/Register` | POST | alta de tercero (valida identificación + tipo) |
| `Productos/Register` | POST | alta de producto (valida referencia/código) |
| `Compras/GetProductsAndProviders` | GET | combos: productos + proveedores (tipo 1) |
| `Compras/Register` | POST | registra compra + actualiza valor y stock |
| `Compras/GetCompras` | POST | listado (filtro opcional por rango de fechas) |
| `Compras/ChangeStatus` | POST | invierte `Estado` (trigger ajusta stock) |
| `Compras/GetData` | GET | datos del dashboard |
| `Ventas/GetProductsAndClients` | GET | combos: productos + clientes (tipo 2) |
| `Ventas/Register` | POST | registra venta + descuenta stock |
| `Ventas/GetVentas` | GET/POST | listado de ventas |
| `Ventas/ChangeStatus` | POST | invierte `Estado` (trigger ajusta stock) |

> El cliente Angular nuevo usa otras rutas (`Products/Create`, `Thirds/Create`,
> `Authentication/Login`, `Authentication/GetProfileCurrentUser`); esta tabla es
> referencia de comportamiento, no un contrato a replicar literalmente.
