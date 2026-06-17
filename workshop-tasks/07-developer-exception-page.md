# Task 07 - Developer Exception Page in Production

**OWASP:** A02 Security Misconfiguration

## Scenario

`app.UseDeveloperExceptionPage()` is registered unconditionally — for every
environment, including production. On any unhandled exception the response
returns full stack traces, internal file paths, framework versions and
potentially configuration values (such as connection strings). The same applies
to `DetailedErrors: true` in `appsettings.json`.

## Where to look

- `src/SecurityLab.Api/Program.cs` — `app.UseDeveloperExceptionPage()` (unconditional)
- `src/SecurityLab.Api/appsettings.json` — `"DetailedErrors": true`

## How to reproduce

Trigger an unhandled exception, for example a malformed SQL injection payload in
the search endpoint (Task 03):

```
GET /api/products/search?name=&sortBy=NoSuchColumn
```

Instead of a generic error you receive a detailed developer error page / JSON
revealing internal implementation details and the failing SQL.

## Symptoms

- Stack traces, file paths and SQL appear in error responses.
- Attackers learn framework versions and internal structure for free.

## Goal

Detailed errors must be available only in development. Production responses must
be generic and must not leak internal state.

## Possible fixes

- Guard the developer page with the environment:
  ```csharp
  if (app.Environment.IsDevelopment())
      app.UseDeveloperExceptionPage();
  else
  {
      app.UseExceptionHandler("/error");
      app.UseHsts();
  }
  ```
- Set `DetailedErrors: false` (or remove it) for non-development configs.
- Add a generic `/error` endpoint that returns a safe message.

## Success criteria

- In `Production`, an unhandled exception returns a generic error with no stack
  trace, paths or SQL.
- In `Development`, detailed errors are still available for debugging.
