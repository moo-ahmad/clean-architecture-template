# Clean Architecture API Template

A reusable **ASP.NET Core 8** solution template organized with **Clean Architecture**. Use it as a starting point for APIs that need structured layers, JWT authentication, SQL Server persistence, and common cross-cutting concerns already wired up.

Clone the repository, rename it to your product name, configure the database, and start building features in the Application layer.

## What's included

| Layer | Project | Responsibility |
| --- | --- | --- |
| **API** | `{ProjectName}.API` | HTTP endpoints, middleware, Swagger, DI composition |
| **Application** | `{ProjectName}.Application` | Commands/queries (MediatR), DTOs, interfaces |
| **Domain** | `{ProjectName}.Domain` | Core types, enums, shared primitives |
| **Infrastructure** | `{ProjectName}.Infrastructure` | EF Core, ASP.NET Identity, repositories, auth services, Dapper |

**Built-in features**

- [MediatR](https://github.com/jbogard/MediatR) for request/handler workflows (CQRS-style)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) with SQL Server and migrations
- [ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity) (`User` entity, role seeding migration)
- JWT bearer authentication (register + login sample endpoints)
- Generic repository + unit of work
- Optional [Dapper](https://github.com/DapperLib/Dapper) access via `IDapperRepository`
- [Serilog](https://serilog.net/) (console + rolling file logs)
- Global exception handling middleware
- [Swagger / OpenAPI](https://learn.microsoft.com/aspnet/core/tutorials/web-api-help-pages-using-swagger) in Development

The solution ships with a placeholder name (`ProjName`). Replace it before you start feature work.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or full instance)
- [PowerShell](https://learn.microsoft.com/powershell/) (for the rename script; built in on Windows)
- Optional: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with the C# extension

Verify the SDK:

```bash
dotnet --version
```

## Quick start

### 1. Clone the template

```bash
git clone <your-repo-url>
cd followup-mate-server
```

### 2. Rename the solution

From the repository root, run [`Rename-Project.ps1`](./Rename-Project.ps1). It updates namespaces, usings, project references, configuration values, and file/folder names under `src/`.

Preview changes:

```powershell
.\Rename-Project.ps1 -NewName AcmeBilling -OldName ProjName -WhatIf
```

Apply the rename:

```powershell
.\Rename-Project.ps1 -NewName AcmeBilling -OldName ProjName
```

| Parameter | Description |
| --- | --- |
| `NewName` | **Required.** PascalCase name (e.g. `AcmeBilling`). Used for assemblies, namespaces, and the default database catalog name. |
| `OldName` | Placeholder to replace. Default in the script is `FollowUpMate`; use `ProjName` for this repository copy. |
| `SourceRoot` | Path to the folder containing the `.sln`. Defaults to `./src`. |
| `WhatIf` | Dry run — lists changes without writing files. |

After renaming, open `src/AcmeBilling.sln` (paths match your `NewName`).

### 3. Configure the database and JWT

Edit `src/{ProjectName}/appsettings.Development.json` (and `appsettings.json` for other environments):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AcmeBilling;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "AcmeBilling",
    "ExpiryMinutes": 3600,
    "Key": "<at-least-32-characters-secret-key>"
  }
}
```

Replace `Server`, authentication mode, and `Key` with values appropriate for your environment. Never commit production secrets.

### 4. Apply database migrations

From the repository root:

```bash
dotnet ef database update \
  --project src/AcmeBilling.Infrastructure/AcmeBilling.Infrastructure.csproj \
  --startup-project src/AcmeBilling/AcmeBilling.API.csproj
```

This creates the database and applies the included migrations (schema + role seed).

To add a new migration later:

```bash
dotnet ef migrations add <MigrationName> \
  --project src/AcmeBilling.Infrastructure/AcmeBilling.Infrastructure.csproj \
  --startup-project src/AcmeBilling/AcmeBilling.API.csproj \
  --output-dir Migrations
```

### 5. Run the API

```bash
dotnet run --project src/AcmeBilling/AcmeBilling.API.csproj
```

In Development, Swagger is available at:

- HTTPS: `https://localhost:7174/swagger`
- HTTP: `http://localhost:5186/swagger`

Ports come from `Properties/launchSettings.json`; adjust them if needed.

## Solution layout

```
followup-mate-server/
├── Rename-Project.ps1          # Template rename automation
├── README.md
└── src/
    ├── {ProjectName}.sln
    ├── {ProjectName}/                    # Web API host
    │   ├── Controllers/
    │   ├── Middleware/
    │   ├── Program.cs
    │   └── appsettings*.json
    ├── {ProjectName}.Application/        # Features, handlers, contracts
    ├── {ProjectName}.Domain/             # Domain model
    └── {ProjectName}.Infrastructure/     # Data access, external services
        ├── Data/
        ├── Migrations/
        ├── Repositories/
        └── Services/
```

**Dependency direction:** API → Application → Domain ← Infrastructure (Infrastructure implements Application interfaces and references Application + Domain).

## Sample API

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/auth/register` | Register a user (MediatR `RegisterCommand`) |
| `POST` | `/api/auth/login` | Login and receive a JWT (`LoginCommand`) |
| `GET` | `/weatherforecast` | Sample read endpoint |

Use the `{ProjectName}.http` file in the API project with the REST Client extension (VS Code) or similar tools for quick requests.

## Adding a new feature

1. **Domain** — Add entities or value objects if needed.
2. **Application** — Add command/query, handler, DTOs, and interfaces under `Features/`.
3. **Infrastructure** — Implement interfaces (repositories, services).
4. **API** — Add a thin controller that sends MediatR requests.

Register new Application services in `{ProjectName}.Application/DependencyInjection.cs` and Infrastructure services in `{ProjectName}.Infrastructure/DependecyInjection.cs`. API-level concerns (JWT, MediatR assembly scan) live in `{ProjectName}/DependencyInjection.cs`.

## Configuration reference

| Setting | Location | Notes |
| --- | --- | --- |
| Connection string | `appsettings*.json` → `ConnectionStrings:DefaultConnection` | SQL Server |
| JWT | `Jwt:Issuer`, `Jwt:Key`, `Jwt:ExpiryMinutes` | Used by `JwtTokenGenerator` |
| Serilog | `Serilog` section | Console + daily rolling files under `logs/` |

Logs are written relative to the API working directory when the app runs.

## Troubleshooting

**Build fails after rename**  
Ensure you opened the renamed `.sln` and that all `ProjectReference` paths in `.csproj` files match the new folder names. Re-run `dotnet restore`.

**EF Core tools not found**  
Install the global tool once:

```bash
dotnet tool install --global dotnet-ef
```

**Database connection errors**  
Confirm SQL Server is running, the catalog name exists or can be created, and the connection string matches your auth mode (Windows vs SQL login).

**JWT validation errors**  
`Jwt:Issuer` and the token issuer must match. Use a sufficiently long `Jwt:Key` for HMAC signing.

## License

Add your license here (e.g. MIT) if you publish this template for others to use.
