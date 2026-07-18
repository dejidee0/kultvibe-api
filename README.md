# KultVibe API — Backend

.NET 8 / ASP.NET Core Web API for KultVibe, built on Clean Architecture as specified in
Master Plan v2.0. This scaffold covers **Phase 1: Public Platform + Waitlist**.

## Solution structure

```
KultVibe.sln
└── src/
    ├── KultVibe.API             Controllers, Program.cs, Middleware, Auth (SignalR Hubs land here in Phase 4)
    ├── KultVibe.Application     Services, DTOs, Interfaces, Validators
    ├── KultVibe.Domain          Entities, Enums, Exceptions — zero external dependencies
    ├── KultVibe.Infrastructure  EF Core DbContext, Repositories, Brevo/external API clients
    └── KultVibe.Shared          ApiResponse<T> envelope, PagedResult<T>, cross-cutting constants
```

Dependency direction is one-way: `API → Application/Infrastructure/Shared`, `Infrastructure → Application/Domain/Shared`,
`Application → Domain/Shared`, `Domain` and `Shared` depend on nothing else in the solution.

## What's implemented (Phase 1)

| Endpoint | Auth |
|---|---|
| `POST /api/waitlist` | Public, rate-limited (5/hour/IP) |
| `GET /api/waitlist/count` | Public |
| `GET /api/waitlist/exists/{email}` | Public |
| `GET /api/admin/waitlist` | Basic Auth (`Admin__Username` / `Admin__Password`) |
| `DELETE /api/admin/waitlist/{id}` | Basic Auth |
| `GET /api/health` | Public |

Every response — success or error — is wrapped in `ApiResponse<T>` exactly as specified in Master Plan §2.1.
Validation, conflict, and not-found errors are thrown as typed exceptions from `KultVibe.Domain.Exceptions`
and translated to the right HTTP status by `ExceptionHandlingMiddleware` — controllers and services never
build error JSON by hand.

**Note on the admin endpoints:** the Master Plan says "Basic auth header" for Phase 1 without specifying
where the credentials come from. I added `Admin__Username` / `Admin__Password` as new env vars for this —
they're not in the Master Plan §2.5 table, so add them to production config alongside the documented ones.
This gate gets replaced by JWT + `Role=Admin` once the Users/Auth system lands in Phase 2.

## Infrastructure (as of Phase 1)

- **API hosting:** SmarterASP.NET (IIS, Web Deploy) — **not** Railway as originally listed in Master Plan §Field
  table. Update noted here since this is a deviation from the documented plan (see Master Plan §1: "Document
  any deviations from this plan in writing before implementing").
- **Database:** SQL Server on SmarterASP.NET (`SQL1002.site4now.net`), managed directly via SSMS.
- **Frontend hosting:** Vercel → kultvibe.com (unchanged from Master Plan).
- **Environments:** single environment only (free trial tier has no staging slot) — local dev machine,
  then straight to the SmarterASP production database. No automated backup on the free tier, so any
  migration that touches existing data gets tested against a local Docker SQL Server first, and a manual
  `.bacpac` export (SSMS → Tasks → Export Data-tier Application) is taken before applying it to prod.
- **Redis (Phase 4+, SignalR backplane):** SmarterASP doesn't provide this — plan is an external managed
  Redis (e.g. Upstash) when we get there.

## Prerequisites

- .NET 8 SDK
- SQL Server for local dev (Docker container recommended — keeps prod DB untouched during development)
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef` (skip if already installed)
- SSMS (SQL Server Management Studio) for connecting to the SmarterASP production database directly

## First-time setup

```bash
# from the repo root
dotnet restore

# generate the initial migration (Migrations/ folder is currently empty on purpose)
dotnet ef migrations add InitialCreate \
  --project src/KultVibe.Infrastructure \
  --startup-project src/KultVibe.API

# apply it — Program.cs also does this automatically on startup, but running it
# explicitly first is a good sanity check
dotnet ef database update \
  --project src/KultVibe.Infrastructure \
  --startup-project src/KultVibe.API
```

Set your local connection string and admin credentials in
`src/KultVibe.API/appsettings.Development.json` (already scaffolded with placeholders), or via
user-secrets:

```bash
cd src/KultVibe.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=KultVibe;User Id=sa;Password=...;TrustServerCertificate=True;"
dotnet user-secrets set "Brevo:ApiKey" "xkeysib-..."
dotnet user-secrets set "Admin:Username" "codemonk"
dotnet user-secrets set "Admin:Password" "something-strong"
```

## Run

```bash
dotnet run --project src/KultVibe.API
```

Swagger UI opens automatically in Development at `/swagger`. Health check: `GET /api/health`.

## Git workflow

- `main` = whatever is currently deployed to SmarterASP production.
- `dev` = integration branch. Feature branches off `dev`, merged back via PR (self-merged is fine solo —
  it's for clean history and an easy revert point, not review gatekeeping).
- `dev` → `main` at each phase's deploy checkpoint, matching the Master Plan's "deploy at the end of every
  phase" rule.
- Commit convention: `feat:`, `fix:`, `chore:`, referencing the phase/endpoint where useful
  (e.g. `feat(waitlist): add rate limiting to signup endpoint`).
- This repo is backend-only by design — David (frontend) has no access here, matching the co-founder
  agreement's ownership split. `Kultvibe-web` is the separate frontend repo.
- **Swagger UI is the source of truth for what's actually live** — point David at the deployed
  `/swagger` endpoint rather than this repo for API contract questions.

## Deploying to SmarterASP.NET (Web Deploy)

Production config lives in `appsettings.Production.json`, which is **gitignored and never committed**.
Copy the template first:

```bash
cp src/KultVibe.API/appsettings.Production.json.example src/KultVibe.API/appsettings.Production.json
# then fill in the real values - connection string, Brevo key, Admin credentials
```

**Publish:**

```bash
dotnet publish src/KultVibe.API/KultVibe.API.csproj -c Release -o ./publish
```

**Deploy via Web Deploy** — check the SmarterASP control panel first for a downloadable `.PublishSettings`
file; importing that into Visual Studio's Publish profile is safer than hand-typing credentials into a
`.pubxml` (which would risk getting committed if `Properties/PublishProfiles/` isn't gitignored — it already
is, but double-check before your first publish). If no `.PublishSettings` file is available, configure the
profile manually with:

- Service URL: `https://win1034.site4now.net:8172/MsDeploy.axd?site=kultvibe-001-site1`
- Site name: `kultvibe-001-site1`
- Destination URL (temp, until `kultvibe.com` DNS points here): `http://kultvibe-001-site1.ftempurl.com/`

**Before every migration against prod:**
1. Run it against a local Docker SQL Server first and confirm it does what you expect.
2. If it touches existing data, export a `.bacpac` via SSMS (Tasks → Export Data-tier Application) as a
   manual backup — the free tier has no automated backup.
3. Then apply it to the SmarterASP database.

`Program.cs` calls `db.Database.Migrate()` on startup, so once the migration is committed and the app is
republished, it applies automatically — no separate manual `dotnet ef database update` step needed against
prod, though running it once yourself first (per the migration workflow above) is the safer sequence.

**IIS note:** the API project is configured for `InProcess` hosting (`AspNetCoreHostingModel` in
`KultVibe.API.csproj`), which is the standard, faster option for shared IIS hosting. `dotnet publish`
auto-generates the `web.config` IIS needs — no manual IIS configuration should be required beyond having
the ASP.NET Core Hosting Bundle available on the SmarterASP server (worth confirming with their support if
the app doesn't start after first deploy).

## What's deliberately NOT here yet

Per the Master Plan's phase ordering — "Build in this exact phase order — no skipping ahead" — this scaffold
has no Users/Auth, JWT, OAuth, ELO, wallet, tournaments, streaming, or SignalR hubs. Those land in Phases 2–6.
`ApplicationDbContext` has a comment marking exactly where each phase's `DbSet`s get added.

## Next up

Phase 2 (Weeks 5–8): Auth system (JWT + Google/Discord OAuth) and the ELO engine. Say the word when you're
ready to start on it — the `Users`, `RefreshTokens`, and `OtpCodes` tables are already specified in Master
Plan §4.1.
