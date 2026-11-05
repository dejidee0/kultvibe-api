# KultVibe backend status

Updated: 2026-11-05

Current phase: Phase 1, waitlist.

Backend deployed: see DECISIONS.md

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

- Verify remote dev publication, production configuration, CORS origin, TLS, and local publish output for this release.
- Deploy the backend to SmarterASP only after the readiness gates pass, then run the authorized live smoke test and delete its test row.
- Configure the frontend's public API URL before its future production build. Frontend deployment and Vercel settings are outside this run.
- Resolve the open verticals decision with David; retain existing enum values once real signups exist.

## Run locally

1. Install the .NET 8 SDK and run `dotnet restore`, then `dotnet build` from the repository root.
2. Configure the intended local SQL database and email settings through untracked local configuration or process environment variables. Do not commit credentials.
3. In PowerShell, set `$env:ASPNETCORE_ENVIRONMENT="Development"` and run `dotnet run --project src/KultVibe.API --no-launch-profile --urls http://localhost:5080`.
4. The Development CORS configuration includes the frontend on localhost or 127.0.0.1, port 3000. Health is available at `/api/health`.

Startup calls EF migrations against the configured database. Confirm the intended local database before running; publishing alone does not start the API.

## Deployment targets

- Backend: SmarterASP, IIS with in-process ASP.NET Core hosting, deployed through Web Deploy.
- Frontend: Vercel, maintained in the sibling Kultvibe-web repository. Do not deploy the frontend or change Vercel settings as part of the backend deployment run.
- Production configuration and publish profiles remain ignored; local artifacts and publish-out are ignored.
