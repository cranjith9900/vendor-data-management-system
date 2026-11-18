# Vendor Data Management System (VDMS)

A full-stack sample matching the VDMS project stack: **ASP.NET Core Web API + Entity Framework Core + SQL Server**, with an **Angular 15** frontend (HTML/CSS/Bootstrap/JavaScript) for transaction views, reports and graphs.

## Structure

```
VDMS/
  server/VDMS.Api/     ASP.NET Core 8 Web API (Parts, Transactions, Vendors, Reports)
  vdms-client/         Angular 15 app (routing, forms, Chart.js reports)
```

## Domain

- **Vendor** — supplier of parts
- **Part** — part/inventory record (number, description, category, price, qty on hand, warehouse location), linked to a Vendor
- **Transaction** — Inbound / Outbound / Adjustment movement against a Part; posting one automatically updates the part's quantity on hand
- **Reports** — inventory value by category, transaction trend (last N days), top vendors by inventory value

## Backend — run the API

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download) (not installed in this environment, so it hasn't been compiled here — the code is a complete, standard `dotnet new webapi` + EF Core layout).

```bash
cd server/VDMS.Api
dotnet restore
dotnet run
```

By default (`appsettings.json` → `"UseSqlite": true`) it runs against a local SQLite file (`vdms.db`) so it works with zero extra setup — on startup it calls `EnsureCreated()` and seeds 2 vendors + 3 parts.

To use **SQL Server** as described in the stack:

1. In `appsettings.json`, set `"UseSqlite": false`.
2. Point `ConnectionStrings:SqlServer` at your instance (default assumes LocalDB).
3. Optionally switch `EnsureCreated()` to real migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

API runs at `http://localhost:5080`, with Swagger UI at `http://localhost:5080/swagger`.

### Endpoints

- `GET/POST /api/vendors`, `GET/PUT/DELETE /api/vendors/{id}`
- `GET/POST /api/parts` (supports `?search=` and `?category=`), `GET/PUT/DELETE /api/parts/{id}`
- `GET/POST /api/transactions` (supports `?partId=`)
- `GET /api/reports/inventory-summary`, `/api/reports/transaction-trend?days=30`, `/api/reports/top-vendors?take=5`

## Frontend — run the Angular app

```bash
cd vdms-client
npm install
npm start
```

Opens at `http://localhost:4200`. It calls the API at `http://localhost:5080/api` (see `src/environments/environment.ts`); CORS is already enabled on the API for `http://localhost:4200`.

Screens:
- **Parts** — searchable list, add/edit/delete
- **Transactions** — list + post new inbound/outbound/adjustment transactions
- **Reports** — bar/pie/line charts (Chart.js) for inventory value by category, by vendor, and transaction trend

## Notes

- The Angular production build was verified locally (`npx ng build`) and compiles cleanly.
- The .NET API could not be compiled in this environment because the .NET SDK isn't installed here; install it and run `dotnet build` inside `server/VDMS.Api` to verify.
