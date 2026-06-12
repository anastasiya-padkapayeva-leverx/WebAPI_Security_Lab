# Task 07 - Sessions Not Revoked on Password Change

**OWASP:** A07 Identification and Authentication Failures

## Scenario

When a user changes their password, the API updates the hash but does **not**
increment the user's `TokenVersion`. The application already enforces token
versioning in `OnTokenValidated`, so the fix is to bump the version on password
change — which invalidates every previously issued JWT. Today, a token stolen
before the change keeps working afterwards.

## Endpoint

```
POST /api/auth/change-password
```

## Where to look

- `src/SecurityLab.Api/Application/Services/AuthService.cs` — `ChangePasswordAsync`
- Token-version enforcement is in `src/SecurityLab.Api/Infrastructure/DependencyInjection.cs`
  (`OnTokenValidated` compares the `ver` claim to `user.TokenVersion`).

## How to reproduce

1. Log in as **alice** and copy the token (call it `T1`). Simulate that `T1` is
   the *attacker's* stolen token.
2. As alice, change the password:
   ```
   POST /api/auth/change-password
   { "currentPassword": "Password1!", "newPassword": "NewPassw0rd!" }
   ```
3. Reuse the old token `T1`:
   ```
   GET /api/auth/me
   Authorization: Bearer T1
   ```
   It **still works** — the password change did not revoke the old session.

## Symptoms

- Old JWTs remain valid after a password reset/change.
- A compromised session survives the very action meant to recover the account.

## Goal

Changing the password must invalidate all previously issued tokens, while the
user who performed the change gets a fresh working token.

## Possible fixes

- In `ChangePassword`, increment `user.TokenVersion` before `SaveChangesAsync`.
- Issue and return a **new** token (with the new version) so the current client
  stays logged in.
- (The `OnTokenValidated` check already rejects tokens whose `ver` no longer
  matches.)

## Success criteria

- After a password change, the old token `T1` returns `401`.
- A token issued by the change-password response works.
- A normal login afterwards works.
