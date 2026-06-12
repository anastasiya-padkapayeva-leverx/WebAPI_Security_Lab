# Task 03 - IDOR: Broken Object-Level Access Control

**OWASP:** A01 Broken Access Control

## Scenario

The orders endpoints load an order by its `id` only. They never check that the
order belongs to the caller, so any authenticated user can read or cancel any
order by changing the id in the URL. This is an **Insecure Direct Object
Reference (IDOR)**.

## Endpoints

```
GET  /api/orders/{orderId}
POST /api/orders/{orderId}/cancel
```

(require a valid token)

## Where to look

- `src/SecurityLab.Api/Application/Services/OrderService.cs` — `GetByIdAsync`, `CancelAsync`

## How to reproduce

1. Log in as **alice** (`POST /api/auth/login`) and copy the token.
2. Alice owns order **1**, but request **Bob's** order:
   ```
   GET /api/orders/2
   ```
   You receive Bob's order details.
3. Now cancel Bob's order as Alice:
   ```
   POST /api/orders/2/cancel
   ```
   The status flips to `Cancelled` even though the order is not Alice's.

## Symptoms

- A user can read/modify resources owned by another user.
- Iterating ids (1, 2, 3, …) enumerates other users' data.

## Goal

Every read and write must be scoped to the resources the caller actually owns.

## Possible fixes

- Filter by the current user id in every query:
  `o => o.Id == orderId && o.UserId == currentUserId`.
- Get the user id from `User.FindFirstValue(ClaimTypes.NameIdentifier)`.
- Optionally enforce this at the data layer with an EF Core **global query
  filter** so it cannot be forgotten.

## Success criteria

- Alice gets `404 Not Found` for order **2** (Bob's).
- Alice can still read and cancel her own order **1**.
- Cancelling another user's order is impossible.
