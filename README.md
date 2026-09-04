# LuxuryBiker

Sistema de punto de venta (POS). Migración del proyecto legado (.NET multicapa + React)
a **.NET Clean Architecture + Angular**.

- `LuxuryBiker.Backend/` — API .NET 8 (Clean Architecture: Domain / Application / Infrastructure / Api).
- `LuxuryBiker.ClientApp/` — SPA Angular 17 (arquitectura hexagonal: domain / data / presentation), tema Melody.
- `docker-compose.yml` — SQL Server 2022 para desarrollo.
- `CONTEXTO-MIGRACION.md` — estado del proyecto, roadmap y deudas técnicas.

## Requisitos

| Herramienta | Versión probada |
|---|---|
| .NET SDK | 8.0 |
| Node.js | 20.x |
| Angular CLI | 17.x (`npx ng`) |
| Docker Desktop | para el contenedor de SQL Server |
| `dotnet-ef` | 8.x (`dotnet tool update --global dotnet-ef --version 8.*`) |

## 1. Base de datos (Docker)

```bash
cd LuxuryBikerAngularNet
cp .env.example .env          # opcional: ajusta MSSQL_SA_PASSWORD
docker compose up -d          # levanta SQL Server en localhost:1433 (usuario: sa)
```

La contraseña por defecto es `LuxuryBiker_2024!` y debe coincidir con la cadena de
conexión de `LuxuryBiker.Backend/LuxuryBiker.Api/appsettings.Development.json`
(clave `ConnectionStrings:LuxuryBiker`).

> Si Docker Desktop no está corriendo, `docker compose` fallará con
> *"cannot find the file specified"*: abre Docker Desktop y reintenta.

## 2. Backend (API)

```bash
cd LuxuryBiker.Backend
dotnet restore
dotnet build
dotnet test          # pruebas unitarias (LuxuryBiker.Application.UnitTests)
dotnet run --project LuxuryBiker.Api
```

- En entorno `Development` la API **aplica migraciones y siembra datos** al arrancar
  (`InitialiseDatabaseAsync`).
- Swagger: `https://localhost:7283/swagger`
- Usuario semilla: `administrator@luxurybiker.com` / `Administrator1!`

Migraciones manuales (si se necesita):

```bash
dotnet ef database update --project LuxuryBiker.Infrastructure --startup-project LuxuryBiker.Api
```

## 3. Frontend (Angular)

```bash
cd LuxuryBiker.ClientApp
npm install
npm start                     # ng serve -> http://localhost:4200
```

`src/environments/environment.ts` apunta a `https://localhost:7283`. La build de
producción usa `environment.prod.ts` vía `fileReplacements` en `angular.json`.

```bash
npm run build                 # build de producción en dist/wwwroot
```

## Estructura del backend

```
LuxuryBiker.Domain          Entidades, eventos de dominio, interfaces de repositorio, constantes
LuxuryBiker.Application      Commands/Queries (MediatR), DTOs, behaviors de pipeline, interfaces de servicios
LuxuryBiker.Infrastructure   EF Core (DbContext, metadata, migraciones), repositorios, servicios (JWT, Identity)
LuxuryBiker.Api             Controllers, Swagger, CORS, middleware de excepciones
```

## Estructura del frontend

```
src/base            Abstracciones (UseCase, Mapper)
src/domain          Modelos, interfaces de repositorio y casos de uso (sin Angular)
src/data            Repositorios HTTP, mappers entity<->model, providers de DI
src/presentation    App Angular: componentes, layouts Melody, servicios, guards, i18n
```

## Notas

- Secreto JWT de `appsettings.Development.json` es solo para desarrollo local. Antes de
  cualquier despliegue, moverlo a User Secrets / variables de entorno.
- `AutoMapper 13.0.1` tiene un aviso de auditoría NuGet (NU1903) pendiente de revisar.
- La build de producción de Angular sube de 5 MB por los bundles del tema Melody
  (presupuesto ampliado en `angular.json`); pendiente adelgazar vendors.
- CI en `.github/workflows/ci.yml`: job de backend (`dotnet build` + `dotnet test` en
  Release) y job de frontend (`npm ci` + `npm run build`).
