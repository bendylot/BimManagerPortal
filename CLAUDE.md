# BIM Manager Portal — контекст проекта

## Структура решения

```
src/Core/
  BimManagerPortal.Domain       — сущности, перечисления, интерфейсы
  BimManagerPortal.Application  — MediatR handlers, validators, DTOs
src/Infrastructure/
  BimManagerPortal.Persistance  — EF Core, репозитории, миграции
src/Presentation/
  BimManagerPortal.WebApi       — ASP.NET 10 Controllers
  BimManagerPortal.WebAssembly  — Blazor WASM frontend
src/Shared/
  BimManagerPortal.Shared       — DTOs, интерфейсы, контракты
tests/
  BimManagerPortal.Tests        — xUnit + Moq + FluentAssertions
```

## Стек

- **.NET 10**, C# (implicit usings, nullable enabled везде)
- **Blazor WebAssembly** (standalone, не hosted)
- **PostgreSQL 16** через EF Core 10 + Npgsql
- **MediatR 14** — CQRS для всей бизнес-логики
- **FluentValidation 12** — в MediatR pipeline
- **AutoMapper 12** — профили через `IMapFrom<T>`
- **Serilog** → Console + File (rolling daily) + Seq
- **Swagger/Swashbuckle 10**

## Архитектура

**Clean Architecture:** Domain → Application → Infrastructure → Presentation

**CQRS через MediatR:**
- `Features/[Feature]/Commands/[Action]/[Action]Command.cs` + Handler
- `Features/[Feature]/Queries/[Action]/[Action]Query.cs` + Handler
- `LoggingBehavior<,>` как pipeline behavior

**Repository pattern:**
- `IUnitOfWork` → `IGenericRepository<T>` (Add/Update/Delete/GetAll/GetById/Entities)
- `UnitOfWork` кэширует репозитории в Hashtable
- `db.Database.Migrate()` автоматически при старте

**Конфигурации EF:** `IEntityTypeConfiguration<T>`, `ApplyConfigurationsFromAssembly()`

## Аутентификация

**Флоу:** Google OAuth 2.0 → создание/обновление User в БД → JWT (30 дней) → redirect `/auth/callback?token=JWT`

**На клиенте:** `CustomAuthStateProvider` читает `localStorage["authToken"]`, парсит JWT payload вручную (Base64), проверяет `exp`.

**Ограничение:** только `*.softapro@*` email домены разрешены.

**User.Role:** `Unregistered | User | Admin`

## API Controllers

- `GET/GET /api/v1/auth/google` + `/google/finalize` + `/me` + `PUT /profile`
- `CRUD /api/v1/public/plugin-configurations`
- `CRUD /api/v1/public/plugin-big-datas`
- `CRUD /api/v1/public/error-dictionaries`

Return types: `TypedResults.Ok/BadRequest/NotFound/Problem`

CORS: AllowAll (любой origin/method/header)

## Blazor → API

Типизированные HttpClient сервисы: `IPluginConfigurationService`, `IErrorDictionaryService`, `IPluginReportProviderServiceProvider`.

Регистрация: `AddHttpClient<IService, Service>(client => client.BaseAddress = baseAddress)`.

## Тесты

- Handlers тестируются через мок `IUnitOfWork`
- `ICompressionService` мокируется отдельно (GZip для BigData)
- `ApplicationDbContext` через `UseInMemoryDatabase` для репозиториев
- GlobalUsings: `Moq`, `Xunit`, `FluentAssertions`

## Окружения

| Env | БД | Seq |
|---|---|---|
| Development | localhost:5432 | localhost:5341 |
| Docker | postgres:5432 | seq:5341 |

`AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true)` — включён глобально.

Секреты через `.env` → `POSTGRES_*`, `GOOGLE_CLIENT_ID/SECRET`, `JWT_SECRET`.
