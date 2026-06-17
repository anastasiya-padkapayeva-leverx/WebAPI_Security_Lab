# Task 01 - Broken JWT Validation

**OWASP:** A07 Identification and Authentication Failures

## Scenario

JWT bearer authentication is configured with every validation check turned off:
signature, issuer, audience and lifetime are all ignored, and unsigned tokens
are accepted. An attacker can forge a token with any claims — including
`role = Admin` — and the API trusts it.

## Where to look

- `src/SecurityLab.Api/Program.cs` — `AddJwtBearer` / `TokenValidationParameters`

## How to reproduce

1. Build an **unsigned** JWT (alg `none`) with a forged payload, for example:
   - Header: `{"alg":"none","typ":"JWT"}`
   - Payload includes the admin's id and `"role":"Admin"` plus the matching
     `"ver"` claim.
   You can craft one at jwt.io (leave the signature empty) or with a script.
2. Call a protected admin endpoint with that token:
   ```
   GET /api/admin/users
   Authorization: Bearer <forged-token>
   ```
   It is accepted even though you never logged in.

> Note: token-version enforcement (`OnTokenValidated`) still runs, so use a
> `ver` value that matches the target user (default `1`). Fixing this task is
> about making the **signature/issuer/audience/lifetime** checks real.

## Symptoms

- Tokens that were never issued by the server are accepted.
- Expired tokens keep working (`ValidateLifetime = false`).
- Changing `role` in the payload changes the caller's privileges.

## Goal

Only tokens that are correctly signed by the server, with a valid issuer,
audience and unexpired lifetime, may be accepted.

## Possible fixes

- Set `ValidateIssuerSigningKey`, `ValidateIssuer`, `ValidateAudience`,
  `ValidateLifetime` and `RequireSignedTokens` to `true`.
- Provide `IssuerSigningKey` from `Jwt:Key`, `ValidIssuer` from `Jwt:Issuer`,
  `ValidAudience` from `Jwt:Audience`.
- Remove the custom `SignatureValidator` that bypasses signature checking.

## Success criteria

- A forged/unsigned token is rejected with `401`.
- A token signed with the wrong key is rejected.
- A genuine token from `/api/auth/login` still works.
