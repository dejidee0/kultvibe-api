# KultVibe backend status

Updated: 2026-11-06

Current phase: Phase 1, waitlist.

Backend is live at https://api.kultvibe.com; see the live deployment details below.

## Done

- Clean Architecture layers: API, Application, Domain, Infrastructure, Shared.
- Waitlist signup, count, existence lookup, and Basic-authenticated admin list/delete endpoints.
- InitialCreate migration generated and previously verified as applied to the intended database.
- Signup rate limit: 10 requests per hour per client IP, no queue, JSON ApiResponse on rejection.
- Named AdminBasic options binding fixed.
- Confirmation-email acceptance flag persisted after a successful Brevo send; send failures do not fail signup.
- Branded responsive HTML confirmation with HTML-encoded personalization and a plain-text part.
- Swagger controlled by Development environment or Swagger:Enabled.
- Local backend build passed with zero errors and zero warnings; prior local smoke tests and recipient-confirmed email delivery passed.

## Pending

- Check the HTTPS certificate renewal in late January 2027.
- Track the low-priority HEAD health-check response (405).
- Resolve the open six-verticals versus two-pillars decision with David; preserve existing enum values.

## Live deployment

- Backend: https://api.kultvibe.com, hosted on SmarterASP. DNS is CNAME `api` -> `mykultvibe-001-site1.ktempurl.com`; DNS is managed in Vercel because the domain uses Vercel nameservers, and the domain is registered at Namecheap.
- HTTPS uses a SmarterASP free Let's Encrypt certificate, expiring 2027-02-04. Check renewal in late January 2027.
- The API serves `/.well-known` before HTTPS redirection so ACME certificate validation and renewal work.
- Hosting-level Basic authentication on the SmarterASP site is disabled; it had blocked all requests with HTTP 401. The API's admin endpoints retain their own authentication.
- Production CORS allows https://kultvibe.com, https://www.kultvibe.com, and https://kultvibe.vercel.app. The apex domain redirects to https://www.kultvibe.com.
- SmarterASP server IP 208.98.35.144 is authorised in Brevo. Live confirmation-email delivery was verified in a real inbox.
- Known low-priority issue: `HEAD /api/health` returns 405; `GET /api/health` returns 200.
- Open decision: six verticals versus two pillars (gaming and creators, with streaming as a bridge), pending discussion with David. Preserve the existing enum values while that decision is pending.

## Run locally

1. Install the .NET 8 SDK and run `dotnet restore`, then `dotnet build` from the repository root.
2. Configure the intended local SQL database and email settings through untracked local configuration or process environment variables. Do not commit credentials.
3. In PowerShell, set `$env:ASPNETCORE_ENVIRONMENT="Development"` and run `dotnet run --project src/KultVibe.API --no-launch-profile --urls http://localhost:5080`.
4. The Development CORS configuration includes the frontend on localhost or 127.0.0.1, port 3000. Health is available at `/api/health`.

Startup calls EF migrations against the configured database. Confirm the intended local database before running; publishing alone does not start the API.

## Deployment targets

- Backend: SmarterASP, IIS with in-process ASP.NET Core hosting, deployed through Web Deploy.
- Frontend: Vercel, maintained in the sibling Kultvibe-web repository; merging that repository into main triggers its production deploy.
- Production configuration and publish profiles remain ignored; local artifacts and publish-out are ignored.
