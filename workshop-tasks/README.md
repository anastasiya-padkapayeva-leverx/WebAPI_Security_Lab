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

| #   | OWASP | Task                                                                        |
| --- | ----- | --------------------------------------------------------------------------- |
| 01  | A05   | [SQL Injection](01-sql-injection.md)                                        |
| 02  | A02   | [Secrets in Configuration](02-secrets-in-configuration.md)                  |
| 03  | A01   | [IDOR — Broken Object-Level Access Control](03-idor-broken-access-control.md) |
| 04  | A01   | [Missing Function-Level Authorization](04-function-level-authorization.md)  |
| 05  | A07   | [Broken JWT Validation](05-broken-jwt-validation.md)                        |
| 06  | A07   | [No Brute-Force Protection](06-no-brute-force-protection.md)                |
| 07  | A07   | [Sessions Not Revoked on Password Change](07-session-not-revoked.md)        |
| 08  | A02   | [Developer Exception Page in Production](08-developer-exception-page.md)    |
| 09  | A02   | [Overly Permissive CORS](09-permissive-cors.md)                             |

