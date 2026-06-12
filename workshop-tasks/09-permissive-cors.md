# Task 09 - Overly Permissive CORS

**OWASP:** A02 Security Misconfiguration

## Scenario

The default CORS policy uses `AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()`.
Any website on the internet can call this API from a victim's browser and read
the responses. If the API is later moved to cookie-based auth, this also enables
cross-site request abuse.

## Where to look

- `src/SecurityLab.Api/Infrastructure/DependencyInjection.cs` — `AddCors`

## How to reproduce

Send a request with an arbitrary `Origin` header and inspect the response CORS
headers:

```powershell
Invoke-WebRequest http://localhost:5080/api/products `
  -Headers @{ Origin = "https://evil.example" } |
  Select-Object -ExpandProperty Headers
```

The response includes `Access-Control-Allow-Origin: *` (or echoes the origin),
showing that any site is trusted.

## Symptoms

- `Access-Control-Allow-Origin` accepts any origin.
- A malicious page can script authenticated calls and read the results.

## Goal

Only trusted, explicitly listed front-end origins may call the API from a browser.

## Possible fixes

- Replace `AllowAnyOrigin()` with `WithOrigins("https://app.yourcompany.com", ...)`.
- Restrict methods/headers to what the front-end actually needs.
- Only use `AllowCredentials()` together with explicit origins (never with
  `AllowAnyOrigin`).

## Success criteria

- Requests from unlisted origins do not receive permissive CORS headers.
- The approved front-end origin(s) still work.
- `AllowAnyOrigin` no longer appears in the configuration.
