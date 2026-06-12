# Web API Security Lab

A deliberately **insecure** ASP.NET Core Web API for a hands-on security
workshop. Attendees reproduce, locate and fix 9 vulnerabilities drawn from the
OWASP Top 10 categories covered in the workshop article (A01, A02, A05, A07).

> ⚠️ **Intentionally vulnerable.** Run it only on your local machine or an
> isolated environment. Never deploy it or expose it to a network.

## Tech stack

| Concern        | Choice                                                            |
| -------------- | ----------------------------------------------------------------- |
| Runtime        | .NET 8 (LTS); `RollForward=Major` so it also runs on .NET 9/10    |
| Framework      | ASP.NET Core Web API (Controllers)                                |
| API docs / UI  | Swagger / Swashbuckle                                             |
| Data access    | Entity Framework Core                                             |
| Database       | SQLite (file-based, created and seeded automatically — zero setup) |
| Auth           | JWT bearer tokens                                                 |

This stack runs from **VS Code, Visual Studio, Rider and IntelliJ IDEA** with no
external services to install. The SQLite database file is created and seeded on
first run.

## Prerequisites

- .NET SDK 8.0 or newer (`dotnet --version`).
- VS Code: install the **C# Dev Kit** extension (recommended).

## Run it

From the repository root:

```powershell
dotnet run --project .\src\SecurityLab.Api\SecurityLab.Api.csproj
```

Then open Swagger at **http://localhost:5080/swagger**.

- The SQLite DB (`securitylab.db`) is created and seeded automatically.
- To reset all data, stop the app and delete `securitylab.db*`, then run again.

### Running from VS Code

- Press **F5** (the C# Dev Kit creates a launch config), or
- Use the integrated terminal with the `dotnet run` command above.
- Open `requests/SecurityLab.http` and click **Send Request** (REST Client
  extension) to fire the exploit calls.

## Seeded accounts

| Email             | Password     | Role  | Owns order |
| ----------------- | ------------ | ----- | ---------- |
| `admin@lab.local` | `Admin123!`  | Admin | —          |
| `alice@lab.local` | `Password1!` | User  | 1          |
| `bob@lab.local`   | `Password1!` | User  | 2          |

## How to use Swagger with auth

1. `POST /api/auth/login` with one of the accounts above and copy the `token`.
2. Click **Authorize** (top-right), paste the token, and authorize.
3. Call the protected endpoints.

## The vulnerabilities

| #   | OWASP | Vulnerability                            | Main location                          |
| --- | ----- | ---------------------------------------- | -------------------------------------- |
| 01  | A05   | SQL Injection                            | `Services/ProductService.cs`           |
| 02  | A02   | Secrets in configuration                 | `appsettings.json`                     |
| 03  | A01   | IDOR (object-level access)               | `Services/OrderService.cs`             |
| 04  | A01   | Missing function-level authorization     | `Controllers/AdminController.cs`       |
| 05  | A07   | Broken JWT validation                    | `Program.cs`                           |
| 06  | A07   | No brute-force protection                | `Services/AuthService.cs`              |
| 07  | A07   | Sessions not revoked on password change  | `Services/AuthService.cs`, `Program.cs` |
| 08  | A02   | Developer exception page in production   | `Program.cs`, `appsettings.json`       |
| 09  | A02   | Overly permissive CORS                   | `Program.cs`                           |

Every planted flaw is tagged with a `// WORKSHOP:` comment. Searching the
solution for `WORKSHOP` reveals them all — encourage attendees to find them by
testing the API **before** searching.

## Workshop tasks

Detailed, per-vulnerability task sheets (scenario, how to reproduce, where to
look, goal, possible fixes, success criteria) are in
[`workshop-tasks/`](workshop-tasks/README.md).

## Project layout

```
SecurityLab.sln
src/SecurityLab.Api/
  Program.cs               # DI, auth, CORS, error handling
  appsettings.json         # planted secrets (task 02)
  Controllers/             # vulnerable endpoints
  Data/                    # AppDbContext + DbSeeder
  Models/                  # User, Product, Order, OrderItem
  Services/                # AuthService, ProductService, OrderService, TokenService, PasswordHasher
workshop-tasks/            # task specifications
requests/SecurityLab.http  # ready-made exploit requests
```
