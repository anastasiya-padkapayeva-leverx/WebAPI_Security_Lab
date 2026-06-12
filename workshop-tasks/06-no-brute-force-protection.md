# Task 06 - No Brute-Force Protection

**OWASP:** A07 Identification and Authentication Failures

## Scenario

The login endpoint performs no rate limiting, account lockout, or delay. An
attacker can try unlimited passwords as fast as the server responds, enabling
brute-force and credential-stuffing attacks. There is also no password policy on
registration, so trivial passwords are allowed.

## Endpoints

```
POST /api/auth/login
POST /api/auth/register
```

## Where to look

- `src/SecurityLab.Api/Application/Services/AuthService.cs` — `LoginAsync`

## How to reproduce

Hammer the login with wrong passwords and observe that nothing throttles you:

```powershell
1..50 | ForEach-Object {
  try {
    Invoke-RestMethod http://localhost:5080/api/auth/login -Method Post `
      -ContentType application/json `
      -Body '{"email":"alice@lab.local","password":"wrong"}'
  } catch { "attempt $_ : $($_.Exception.Response.StatusCode)" }
}
```

All 50 attempts return immediately with `401` and no lockout — a real attacker
would simply continue until a password works.

## Symptoms

- Unlimited failed logins with no slowdown, lockout or `429` response.
- Registration accepts weak passwords (e.g. `1`).

## Goal

Repeated failed authentication attempts must be throttled and/or locked out, and
weak passwords must be rejected.

## Possible fixes

- Add ASP.NET Core **rate limiting** (`AddRateLimiter` + `[EnableRateLimiting]`)
  on the login endpoint (e.g. 5 attempts / 15 minutes).
- Track failed attempts per account and apply a temporary lockout; return `429`
  when locked.
- Add a uniform small delay and identical error message for unknown user vs.
  wrong password to prevent **user enumeration**.
- Enforce a minimum password policy on registration.

## Success criteria

- After a small number of failed attempts the endpoint returns `429`/lockout.
- Login responses do not reveal whether the email exists.
- Weak passwords are rejected at registration.
