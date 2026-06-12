# Task 01 - SQL Injection

**OWASP:** A05 Injection

## Scenario

The product search endpoint builds a SQL statement by concatenating the raw
`name` and `sortBy` query parameters into a string and executing it with
`FromSqlRaw`. Untrusted input is interpreted as SQL.

## Endpoint

```
GET /api/products/search?name={name}&sortBy={sortBy}
```

(anonymous — no token required)

## Where to look

- `src/SecurityLab.Api/Application/Services/ProductService.cs` — `SearchAsync`

## How to reproduce

Baseline:

```
GET /api/products/search?name=Mouse&sortBy=Name
```

`sortBy` is the cleanest injection point because column names and SQL keywords
cannot be parameterized. Inject an `ORDER BY` expression / comment:

```
GET /api/products/search?name=&sortBy=Price DESC -- 
```

Boolean injection through `name` (returns all rows regardless of the filter):

```
GET /api/products/search?name=%' OR '1'='1&sortBy=Name
```

On SQLite you can also confirm arbitrary expression evaluation via the sort
clause, e.g. `sortBy=(CASE WHEN 1=1 THEN Price ELSE Name END)`.

## Symptoms

- Crafted input changes which/how many rows return, or causes SQL errors.
- The generated SQL is visible in the server console logs (EF Core command logging).

## Goal

User input must never be interpreted as SQL. The `name` filter and the `sortBy`
ordering must both be safe against injection.

## Possible fixes

- Replace the raw SQL with a LINQ query: `_db.Products.Where(p => p.Name.Contains(name))`.
- Implement dynamic sorting with a **whitelist** (`switch` over allowed keys),
  not by concatenating the column name.
- If raw SQL is unavoidable, use `FromSqlInterpolated`/parameters for **values**
  — but remember column names still require a whitelist.

## Success criteria

- `sortBy=Price DESC -- ` no longer changes behavior beyond a known-safe set of options.
- `name=%' OR '1'='1` returns only genuine matches (or none), not the whole table.
- No string concatenation of user input into SQL remains.
