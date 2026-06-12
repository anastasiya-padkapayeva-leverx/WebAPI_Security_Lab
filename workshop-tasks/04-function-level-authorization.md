# Task 04 - Missing Function-Level Authorization

**OWASP:** A01 Broken Access Control (vertical privilege escalation)

## Scenario

The admin controller is protected with `[Authorize]`, which only checks that the
caller is **authenticated** — not that they are an **admin**. Any logged-in user
can list every user (including password hashes) and delete accounts.

## Endpoints

```
GET    /api/admin/users
DELETE /api/admin/users/{id}
```

## Where to look

- `src/SecurityLab.Api/Controllers/AdminController.cs` — `[Authorize]` attribute on the class

## How to reproduce

1. Log in as **alice** (a regular `User`) and copy the token.
2. Call the admin listing:
   ```
   GET /api/admin/users
   ```
   You get all users back, including `PasswordHash` and `TokenVersion`.
3. Delete another account:
   ```
   DELETE /api/admin/users/3
   ```

## Symptoms

- A non-admin can reach admin-only functionality.
- Sensitive fields (password hashes) are exposed to any authenticated caller.

## Goal

Admin endpoints must be reachable only by users in the `Admin` role, and must
not leak sensitive fields.

## Possible fixes

- Require the role: `[Authorize(Roles = "Admin")]` on the controller/actions.
- Remove `PasswordHash` (and other internal fields) from the response — project
  to a safe DTO.
- Consider policy-based authorization for finer control.

## Success criteria

- Alice receives `403 Forbidden` from both admin endpoints.
- `admin@lab.local` can still use them.
- The user listing no longer returns password hashes.
