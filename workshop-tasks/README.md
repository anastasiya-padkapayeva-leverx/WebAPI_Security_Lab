# Workshop Tasks — Web API Security Lab

These tasks map to the OWASP Top 10 categories covered in the workshop article.
The API is **intentionally vulnerable**. Your job for each task is to:

1. **Reproduce** the vulnerability using Swagger or the `requests/SecurityLab.http` file.
2. **Locate** the flaw in code.
3. **Fix** it.
4. **Verify** the exploit no longer works while legitimate use still does.

## Seeded accounts

| Email             | Password     | Role  |
| ----------------- | ------------ | ----- |
| `admin@lab.local` | `Admin123!`  | Admin |
| `alice@lab.local` | `Password1!` | User  |
| `bob@lab.local`   | `Password1!` | User  |

Alice owns order **1**, Bob owns order **2**.

## Task index

> **Recommended order:** complete tasks top-to-bottom. Tasks 03–05 require a
> valid token, so understand authentication (Task 01) first. Task 07 builds
> directly on the JWT token-versioning mechanism introduced in Task 01.

| #   | OWASP | Task                                                                        |
| --- | ----- | --------------------------------------------------------------------------- |
| 01  | A07   | [Broken JWT Validation](01-broken-jwt-validation.md)                        |
| 02  | A02   | [Secrets in Configuration](02-secrets-in-configuration.md)                  |
| 03  | A05   | [SQL Injection](03-sql-injection.md)                                        |
| 04  | A01   | [IDOR — Broken Object-Level Access Control](04-idor-broken-access-control.md) |
| 05  | A01   | [Missing Function-Level Authorization](05-function-level-authorization.md)  |
| 06  | A07   | [Sessions Not Revoked on Password Change](06-session-not-revoked.md)        |
| 07  | A02   | [Developer Exception Page in Production](07-developer-exception-page.md)    |

