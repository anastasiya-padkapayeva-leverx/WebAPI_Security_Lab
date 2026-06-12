# Task 02 - Secrets in Configuration

**OWASP:** A02 Security Misconfiguration

## Scenario

`appsettings.json` is committed to source control with real-looking secrets:
a production database connection string (with `User Id`/`Password`), the JWT
signing key, SMTP credentials, and third-party API keys. Anyone with read
access to the repository (or the deployed file) gets all of them.

## Where to look

- `src/SecurityLab.Api/appsettings.json`

## How to reproduce

- Open `appsettings.json` and read the credentials directly — no exploit needed.
- Combine with **Task 08**: trigger an unhandled exception and the developer
  exception page / detailed errors can surface configuration values to callers.

## Symptoms

- Plaintext passwords, connection strings and API keys live in a tracked file.
- The same `Jwt:Key` is shared by everyone who can see the repo, so anyone can
  mint valid tokens (see Task 05).

## Goal

No secret values should be stored in committed configuration files. The app must
read them from a secure source at runtime.

## Possible fixes

- Remove secrets from `appsettings.json`; keep only non-sensitive structure.
- Use **User Secrets** for local dev (`dotnet user-secrets set "Jwt:Key" "..."`).
- In real deployments, load from environment variables or a secrets manager
  (Azure Key Vault, AWS Secrets Manager, etc.).
- Rotate any secret that was ever committed — treat it as compromised.
- Add the secret files to `.gitignore`.

## Success criteria

- `appsettings.json` contains no passwords, connection strings with credentials,
  or API keys.
- The app still starts because it reads `Jwt:Key` and the connection string from
  user secrets / environment variables.
