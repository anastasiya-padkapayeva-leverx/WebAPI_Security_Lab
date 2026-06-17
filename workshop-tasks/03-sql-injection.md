# Task 03 - SQL Injection

**OWASP:** A03 Injection

## Scenario

Two endpoints build SQL by concatenating raw query parameters into a string
and executing it with `FromSqlRaw`. Untrusted input is interpreted as SQL.

The more dangerous one searches **users**: it is reachable by any authenticated
user (task 04 shows why the admin role is not enforced) and returns e-mails,
password hashes, and roles.

## Endpoints

```
GET /api/admin/users/search?email={email}      ← primary: directly queries Users
GET /api/products/search?name={name}&sortBy={sortBy}   ← secondary: queries Products
```

## Where to look

- `src/SecurityLab.Api/Services/AdminService.cs` — `SearchUsersAsync`
- `src/SecurityLab.Api/Services/ProductService.cs` — `SearchAsync`

## How to reproduce

### Setup — get a token

Register any account and copy the returned token:

```
POST /api/auth/register
{ "email": "attacker@evil.com", "password": "Password1!" }

POST /api/auth/login
{ "email": "attacker@evil.com", "password": "Password1!" }
```

Use the token in the `Authorization: Bearer <token>` header for all requests below.

### Step 1 — normal search (baseline)

```
GET /api/admin/users/search?email=alice
```

Returns only Alice's record. Good.

### Step 2 — boolean injection (dumps the whole Users table)

The template inside `SearchUsersAsync` is:

```csharp
var sql = $"SELECT * FROM Users WHERE Email LIKE '%{email}%'";
```

Payload for `email`:

```
' OR 1=1 -- 
```

The generated SQL becomes:

```sql
SELECT * FROM Users WHERE Email LIKE '%' OR 1=1 -- %'
```

`1=1` is always true; `-- ` comments out the trailing `%'`.
**Every row in the Users table is returned.**

Full request (URL-encoded):

```
GET /api/admin/users/search?email=%27%20OR%201%3D1%20--%20
```

Example response — the attacker receives all accounts:

```json
[
  {
    "id": 1,
    "email": "admin@lab.local",
    "role": "Admin",
    "passwordHash": "$2a$11$rK3vQ...long bcrypt hash...",
    "tokenVersion": 1
  },
  {
    "id": 2,
    "email": "alice@lab.local",
    "role": "User",
    "passwordHash": "$2a$11$Tz9mP...long bcrypt hash...",
    "tokenVersion": 1
  }
]
```

With the hashes the attacker can run an **offline dictionary attack** (Hashcat,
John the Ripper) and recover passwords — especially weak ones — without
sending a single additional request to the server.

### Step 3 — secondary example: `sortBy` injection in products

Column names cannot be parameterized, so the `sortBy` parameter in the
products endpoint is also concatenated directly:

```
GET /api/products/search?name=&sortBy=Price DESC -- 
```

This reorders results in a way the API was not designed to allow.

## Symptoms

- A crafted `email` payload returns far more records than expected.
- The generated SQL is visible in the server console (EF Core command logging).
- Response contains `passwordHash` fields for every user in the system.

## Goal

User input must never be interpreted as SQL in either endpoint.

## Possible fixes

- **`SearchUsersAsync`**: replace `FromSqlRaw` with a LINQ `.Where`:
  ```csharp
  _db.Users.Where(u => u.Email.Contains(email))
  ```
- **`SearchAsync` (products `name`)**: same — use `Where(p => p.Name.Contains(name))`.
- **`SearchAsync` (products `sortBy`)**: use a **whitelist** (`switch` over
  allowed column names); never concatenate the value directly.
- If raw SQL is unavoidable for values, use `FromSqlInterpolated` or explicit
  `SqlParameter` objects — but column names still require a whitelist.

## Success criteria

- `email=' OR 1=1 -- ` returns only rows whose email literally contains that
  string (i.e., none), not the whole table.
- `name=%' OR '1'='1` returns only genuine product matches (or none), not all rows.
- `sortBy=Price DESC -- ` no longer changes behaviour beyond the known-safe set
  of allowed column names.
- No string concatenation of user input into SQL remains in either service.
