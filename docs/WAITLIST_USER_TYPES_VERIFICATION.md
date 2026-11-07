# Waitlist user types verification

Updated: 2026-11-07.

## A5: build, migration and deployment

- Source deployed: `74090cdb54ea60c10a33983e73daa05207203262` on backend dev.
- Backend build: zero errors and zero warnings.
- Isolated checks: 33 passed, with mocked email and no database connections. Covered legacy payloads, validation, normalization, duplicate handling, successful and failed email persistence, summary grouping and EF model constraints.
- Migration: `20261107144016_AddWaitlistUserTypes` generated through the design-time factory without running API startup or connecting to the database.
- Compatibility review: the existing category column becomes nullable; the three profile columns are nullable; the required user type has the SQL default `Gamer`. Existing deployed inserts remain valid.
- Applied the reviewed migration before deploying the new API. Both migrations were listed as applied, and old-code GET health remained HTTP 200.
- Live waitlist count before migration: 2. Existing signups were preserved.
- The originally requested compatibility-test address was already registered. The owner authorized `ifemicheall2@gmail.com` instead; it was confirmed unused before migration.
- Release publication succeeded. Web Deploy dry run and sync each reported 20 changes: 0 added, 4 deleted, 16 updated; 401,714 bytes copied. AppOffline was enabled for safe IIS file replacement. No certificate bypass was needed.
- After deployment: GET health returned HTTP 200; Swagger exposed the new user-type request property and the authenticated summary route.
- Credentials, production configuration and generated local artifacts remain excluded from git.

## Remaining gates

- A6 live compatibility, validation, admin summary, cleanup, Swagger and CORS tests: passed.
- A7 product decisions and status documentation: pending.
- A8 backend main merge: pending all live tests.
- Parts B and C: not started; gated on completion of the preceding part.

## A6: live test results

The owner-authorized replacement address was used for the legacy payload. Eight signup POSTs created four rows, all deleted. Starting count: 2; ending count: 2. No rate-limit test was run and no 429 was received. The two existing entries were preserved.

| Check | Expected | Actual | Result |
|---|---|---|---|
| 1. Old payload | 201 | 201 | PASS |
| 2. New Gamer | 201 | 201 | PASS |
| 3. New Creator | 201 | 201 | PASS |
| 4. New Brand | 201 | 201 | PASS |
| 5. Brand requires company | 400 | 400 | PASS |
| 6. Invalid user type | 400 | 400 | PASS |
| 7. Invalid interest | 400 | 400 | PASS |
| 8. Duplicate Gamer | 409 | 409 | PASS |
| 9a. Admin fields and old-payload default | 200 | 200 | PASS |
| 9b. Summary counts | 200 | 200 | PASS |
| 9c. Summary requires authentication | 401 | 401 | PASS |
| 10. Delete own Brand test row | 200 | 200 | PASS |
| 10. Delete own Creator test row | 200 | 200 | PASS |
| 10. Delete own Gamer test row | 200 | 200 | PASS |
| 10. Delete own Gamer test row | 200 | 200 | PASS |
| 10. Starting count restored | 200 | 200 | PASS |
| 11a. Swagger UI | 200 | 200 | PASS |
| 11b. Swagger fields and nullable legacy category | 200 | 200 | PASS |
| 11c. Canonical frontend CORS | 200/204 and matching origin | 204 | PASS |

Summary response while the four test rows existed:

```json
{
  "success": true,
  "data": {
    "total": 6,
    "byUserType": {
      "Gamer": 4,
      "Creator": 1,
      "Brand": 1
    },
    "byInterest": {
      "Music": 1,
      "Tech": 0,
      "Lifestyle": 0
    },
    "topGames": [
      {
        "game": "COD Mobile",
        "count": 1
      }
    ],
    "byLegacyVertical": {
      "Gaming": 2,
      "Creator": 1
    }
  },
  "message": "Operation successful",
  "errors": null
}
```

Swagger confirmed userType, primaryGame, companyName, interests and nullable primaryVertical on both request and admin-entry schemas, plus the summary route. The summary endpoint also returned 401 without admin authentication. Canonical frontend preflight returned 204 with the matching https://www.kultvibe.com origin.
