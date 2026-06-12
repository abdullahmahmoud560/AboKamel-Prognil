# AboKamel — Technical Audit Report

**Scope:** ASP.NET Core API (`AboKamel.Api`), application/domain/infrastructure layers, `appsettings.json`, deployment notes (`Report.md`).  
**Out of scope:** CI/CD pipelines, general DevOps philosophy.  
**Note:** No `docker-compose.yml`, `Dockerfile`, or backup scripts exist in the repository; infrastructure findings are based on `Report.md` and standard production expectations.

---

## 1. Security Analysis

### 1.1 Critical

| ID | Issue | Location | Recommendation |
|----|-------|----------|----------------|
| S-01 | **Unauthenticated admin bootstrap** — `POST /CreateAdminAccount` creates SuperAdmin with fixed credentials | `AuthController.cs` (76–84), `AuthService.cs` (216–228) | Remove endpoint from production builds or protect with one-time setup token + `[Authorize(Roles = "SuperAdmin")]`; rotate credentials immediately if ever deployed publicly |
| S-02 | **Dashboard write APIs without authorization** — brands, categories, areas, offers, selling units, advertisements, analysis, and most product reads are open | `BrandsController.cs`, `CategoriesController.cs`, `AreasController.cs`, `OffersController.cs`, `SellingUnitsController.cs`, `Advertisements.cs` (dashboard), `AnalysisController.cs`, `ProductsController.cs` (22–41) | Apply `[Authorize(Roles = "SuperAdmin")]` at controller level; use read-only role for GET if needed |
| S-03 | **Unauthenticated notification broadcast** | `Mobile/Notifications/NotificationsController.cs` (24–29) | Require `[Authorize(Roles = "SuperAdmin")]`; scope SignalR groups per user/area |
| S-04 | **Unauthenticated dashboard notification send** | `Dashboard/Notifications/NotificationsController.cs` (20–24) | Add `[Authorize(Roles = "SuperAdmin")]` on `SendToNotificationEntity` |
| S-05 | **Secrets committed to source control** — JWT key, Paymob keys, commented remote DB passwords | `appsettings.json` (4–30) | Move secrets to environment variables or Docker secrets; add `appsettings.*.json` overrides to `.gitignore`; rotate all exposed keys |
| S-06 | **Production DB password in documentation** | `Report.md` (47–52) | Remove password from repo; rotate MySQL password on VPS |

### 1.2 High

| ID | Issue | Location | Recommendation |
|----|-------|----------|----------------|
| S-07 | **Plaintext `CustomPassword` authentication** — login compares custom password in clear text against DB column | `AuthService.cs` (34–42), `ApplicationUser.cs` | Use ASP.NET Identity password hashing only; migrate/remove `CustomPassword` column |
| S-08 | **Hardcoded registration password** — all users created with `P@ssw0rd` | `AuthService.cs` (69–71) | Accept password from secure registration DTO; enforce complexity policy |
| S-09 | **JWT signing key logged to stdout** | `AuthService.cs` (262) | Remove `Console.WriteLine(_jwt.Key)` |
| S-10 | **`[AllowAnonymous]` debt balance by `userId`** — IDOR: any caller can query any user's balance | `DebtsController.cs` (37–44) | Require `[Authorize]` and verify `userId == current user` or SuperAdmin |
| S-11 | **Public role enumeration** | `AuthController.cs` (136–142) — `[Authorize]` commented out | Restore `[Authorize(Roles = "SuperAdmin")]` on `GetRoles` |
| S-12 | **Open SignalR hub** — no hub authorization; `SendFromClient` broadcasts to all | `NotificationHub.cs`, `Program.cs` (46) | Add `[Authorize]` on hub; validate JWT on negotiate; disable client broadcast or restrict to admins |
| S-13 | **Swagger UI enabled in all environments** | `Program.cs` (32–33) | Gate with `if (app.Environment.IsDevelopment())` |
| S-14 | **Exception messages returned to clients** | `GlobalExceptionHandler.cs` (41–42) | Return generic message in production; log full exception server-side only |

### 1.3 Medium

| ID | Issue | Location | Recommendation |
|----|-------|----------|----------------|
| S-15 | **File upload validation is extension-only** — no content-type/magic-byte check; `SaveImageAsync` does not call `ValidateImage` | `ImageService.cs` (27–39, 54–64) | Always validate before save; reject path traversal via `Path.GetFullPath` under `WebRootPath` |
| S-16 | **`ExtractImagePath` trusts URL path** — used on update flows | `ImageService.cs` (103–109), dashboard controllers | Whitelist paths under `images/`; never write outside `wwwroot` |
| S-17 | **No CORS policy defined** | `Program.cs` | Add explicit `AddCors` with allowed origins for mobile/web clients |
| S-18 | **No security headers** (HSTS, `X-Content-Type-Options`, CSP, etc.) | `Program.cs` | Add `UseHsts()` in production and security headers middleware |
| S-19 | **`AllowedHosts: "*"`** | `appsettings.json` (15) | Restrict to production hostname(s) |
| S-20 | **`RequireHttpsMetadata = false`** | `Authentication.cs` (30) | Set `true` in production behind HTTPS |
| S-21 | **`IncludeErrorDetails = true` on JWT** | `Authentication.cs` (29) | Disable in production |
| S-22 | **Seeded review user with known password** | `ReviewUserSeeder.cs` | Disable seeder in production or use random password from env |
| S-23 | **Paymob callback not implemented** | `PaymentService.cs` (55–57) | Implement HMAC verification before marking orders paid |
| S-24 | **Generic `CrudController` has no auth** | `CrudController.cs` | Remove or add global authorization filter if ever used |

### 1.4 Low

| ID | Issue | Location | Recommendation |
|----|-------|----------|----------------|
| S-25 | **SQL injection via raw SQL** — not observed | EF Core LINQ throughout | Continue avoiding `FromSqlRaw` with user input |
| S-26 | **CSRF** — API is JWT-header based; many `[FromForm]` endpoints | Controllers | Low risk for native mobile; add antiforgery if browser cookie sessions are introduced |
| S-27 | **Static files served without auth** | `Program.cs` (40) | Expected for product images; avoid storing sensitive files under `wwwroot` |

### 1.5 Infrastructure / environment misconfigurations

| Severity | Issue | Recommendation |
|----------|-------|----------------|
| **High** | MySQL application user documented as `root` | Create least-privilege DB user for API |
| **High** | phpMyAdmin on same Docker network as production DB (`Report.md`) | Use Compose profile; never expose phpMyAdmin port on `0.0.0.0` |
| **Medium** | Docker/Compose not versioned in repo | Commit `docker-compose.yml` and `Dockerfile` for reproducible deploys |
| **Medium** | Empty MySQL password in local `appsettings.json` (line 7) | Use User Secrets locally; never deploy default connection string |

---

## 2. Performance Analysis

### 2.1 High

| ID | Issue | Location | Impact | Recommendation |
|----|-------|----------|--------|----------------|
| P-01 | **Unbounded product catalog load** — all products + favorites projection, no pagination | `ProductService.cs` (35–77), mobile `GetProductsWithFavorite` | RAM and response time grow linearly with catalog size | Add `Skip`/`Take`, cursor/keyset pagination; cache hot pages |
| P-02 | **N+1 queries at order creation** — one `GetByIdAsync` per cart line for selling unit name | `OrderService.cs` (55–59) | Checkout latency scales with cart items | Preload selling units in one query or include in cart mapping |
| P-03 | **Cartesian explosion on cart load** — multiple `Include`/`ThenInclude` on collections without split query | `CartRepository.cs` (23–36) | High CPU/RAM on large carts with prescriptions | Add `.AsSplitQuery()`; project to DTO instead of full entity graph |
| P-04 | **Dashboard loads all orders with full graph** | `OrderRepository.cs` (48–54) | Memory spikes as order history grows | Paginate; filter by date/status; use lightweight list DTO |
| P-05 | **Fake pagination in `CrudController`** — `page`/`NumberOfItems` ignored | `CrudController.cs` (22–25) | Clients may assume paging while full table is loaded | Implement real paging or remove parameters |

### 2.2 Medium

| ID | Issue | Location | Impact | Recommendation |
|----|-------|----------|--------|----------------|
| P-06 | **No `AsNoTracking` on read-heavy queries** | Orders, favorites, notifications, dashboard lists | Extra change-tracking overhead | Apply `AsNoTracking()` on read-only repository methods |
| P-07 | **Per-operation `SaveChangesAsync` in generic repository** | `Repository.cs` (23–40) | Multiple round-trips per business operation | Use unit-of-work / single `SaveChanges` per request where safe |
| P-08 | **Synchronous `SaveChanges()` in async cart init** | `CartService.cs` (98) | Thread pool blocking under load | Replace with `SaveChangesAsync` |
| P-09 | **`ServerVersion.AutoDetect` on every startup** | `DatabaseConnection.cs` (17) | Extra DB connection and startup delay | Pin MySQL version in configuration |
| P-10 | **Brand detail loads all products** | `BrandRepository.cs` (19–21) | Large payloads for popular brands | Paginate products or return counts only on list endpoints |
| P-11 | **Offer/order/debt list endpoints without API-level limits** | Various dashboard/mobile controllers | Unbounded responses | Enforce max page size (e.g. 50) globally |

### 2.3 Low

| ID | Issue | Location | Recommendation |
|----|-------|----------|----------------|
| P-12 | **In-memory loops for URL generation** on prescription media | `CartService.cs` | Acceptable at small scale |
| P-13 | **Advertisement image I/O in loops** | `AdvertisementService` | Batch or parallelize with limits if upload volume grows |
| P-14 | **No memory leak patterns found** — no `.Result`/`.Wait()` on async | — | Positive finding; maintain async-all-the-way |

### 2.4 Resource misuse under load

| Risk | Cause | Mitigation |
|------|-------|------------|
| API OOM | Full product/order loads + large Include graphs | Pagination, split queries, DTO projections |
| MySQL connection pressure | Chatty saves + unbounded queries | Connection pooling defaults + query limits |
| Disk I/O | Unvalidated concurrent uploads to `wwwroot` | Rate limit uploads; separate object storage at scale |

---

## 3. Database Optimization

### 3.1 Missing indexes (High / Medium)

| Severity | Table / columns | Query pattern | Recommendation |
|----------|-----------------|---------------|----------------|
| **High** | `Favorites.UserId` | `Favorites.Any(f => f.UserId == userId)` in catalog | Add composite index `(UserId, ProductId)` |
| **High** | `Notifications` — `CustomerId`, `AreaId`, `CreatedDate` | User notification lists | Add indexes via fluent API + migration |
| **Medium** | `Orders.Status` | `GetOrdersByStatusAsync`, analytics counts | Index `Status` |
| **Medium** | `Orders.CreatedDate` | Revenue/sales period queries | Index `CreatedDate` (or composite with `Status`) |
| **Medium** | `Addresses (CustomerId, IsPrimary)` | Primary address lookup | Composite index |
| **Medium** | `Products.Name`, `Brand.Name`, `Category.Name` | `Contains` search | Consider full-text index (MySQL `FULLTEXT`) or dedicated search service |
| **Low** | `Debts.DebitCredit` | Credit/debit user lists | Index if lists are slow at scale |

**Evidence:** `CapsulaDbContextModelSnapshot.cs` — `Favorites` has index on `ProductId` only (line 694); `Notifications` has key only (lines 164–166).

### 3.2 N+1 and query shape

| Severity | Issue | Location | Fix |
|----------|-------|----------|-----|
| **High** | Order checkout N+1 | `OrderService.cs` (55–59) | Single query: `WHERE Id IN (...)` for selling units |
| **High** | Multi-collection Include without split | `CartRepository.cs`, `OrderRepository.cs` | `AsSplitQuery()` + narrower selects |
| **Medium** | Analytics loads `OrderItems` with Include before GroupBy | `OrderRepository.cs` (~147+) | Project aggregates in SQL only |

### 3.3 Schema and data-loading

| Severity | Issue | Recommendation |
|----------|-------|----------------|
| **Medium** | Widespread `DeleteBehavior.Cascade` on `Product`, `Order`, `Customer`, `Cart`, etc. | Review cascade paths; soft-delete for orders/customers in production |
| **Medium** | `longtext` on searchable columns (`Advertisements.Name`, notification content) | Use `varchar` with length limits where appropriate |
| **Low** | `CustomPassword` column alongside Identity | Remove after auth migration |
| **Info** | No raw SQL — EF parameterized queries | Maintain current approach |

### 3.4 Query optimization checklist

1. Add indexes above via new EF migration.  
2. Enable split queries on repositories with 2+ collection includes.  
3. Use `AsNoTracking()` for all read-only API paths.  
4. Replace `GetAllAsync()` dashboard calls with paginated repository methods.  
5. For mobile catalog, mirror `SearchProductsAsync` pattern (`Take(12)` + projection) across list endpoints.

---

## 4. Architecture & Scalability

### 4.1 System design evaluation

| Area | Current state | Scalability risk |
|------|---------------|------------------|
| API layer | Monolithic ASP.NET Core, synchronous request handling | Single instance bottleneck; vertical scaling only |
| Data | Single MySQL instance, no read replicas | Read-heavy catalog competes with writes |
| Real-time | SignalR in-process | Does not scale across multiple API instances without backplane |
| Files | Local `wwwroot` storage | Not shared across replicas; backup-dependent |
| Payments | Paymob HTTP call per intention | Blocking; no retry/idempotency visible |
| Caching | None (`IMemoryCache`/Redis absent) | Repeated DB hits for catalog, areas, categories |

### 4.2 Bottlenecks by module

| Module | Bottleneck | Severity |
|--------|------------|----------|
| Mobile products | Full-table catalog query | **High** |
| Cart / checkout | Heavy includes + N+1 + multiple saves | **High** |
| Dashboard orders | Unpaginated full history | **High** |
| Notifications | No indexes; broadcast to all SignalR clients | **Medium** |
| Image service | Synchronous disk I/O per upload | **Medium** |
| Startup seeding | `MigrateAsync` + product seed on every boot path | **Low** (guard with env flag) |

### 4.3 Caching recommendations

| Strategy | Improvement | Trade-off |
|----------|-------------|-----------|
| **IMemoryCache** for areas, categories, brands (read-mostly) | Fewer DB round-trips; faster mobile home | Stale data until invalidation; single-instance only |
| **Redis** (distributed cache + SignalR backplane) | Multi-instance API; shared session/cache | Extra service to operate and secure |
| **HTTP caching** for public product search/newest (short TTL) | Reduced API load | Requires cache-control discipline |

### 4.4 Async / background job improvements

| Job candidate | Why | Trade-off |
|---------------|-----|-----------|
| Product/image import seeding | Long-running; blocks startup today | Requires queue or one-off admin command |
| Paymob webhook processing | Must not block HTTP thread; needs retries | Hangfire/Quartz/worker container adds complexity |
| Bulk notifications | Dashboard send could fan out asynchronously | Eventual delivery; need delivery status tracking |
| Nightly analytics rollups | Avoid heavy aggregates on live orders table | Extra storage for summary tables |

**Current state:** No `IHostedService`, Hangfire, or queue infrastructure in the codebase.

### 4.5 Service boundaries

- Application services directly use `CapsulaDbContext` (`ProductService`, `AdvertisementService`, `AuthService`) alongside repositories — inconsistent boundary makes optimization and testing harder.  
- **Recommendation:** Standardize data access through repositories or a single application unit-of-work per request.

---

## 5. Infrastructure & Operations

### 5.1 Docker and deployment (from `Report.md`)

| Severity | Finding | Recommendation |
|----------|---------|----------------|
| **High** | Compose/Docker files not in repository | Version control `docker-compose.yml`, `Dockerfile`, `.env.example` |
| **High** | MySQL `root` for application | Dedicated DB user with minimal grants |
| **High** | phpMyAdmin on production network | Profile-gated; no public port binding |
| **Medium** | No documented CPU/RAM limits on containers | Add `deploy.resources.limits` to prevent OOM cascade |
| **Medium** | API uploads may live only in container filesystem | Bind mount or volume for `wwwroot/images`; include in backup |
| **Low** | Localhost binding (3306, 5000) documented | Verify with `ss -tulpn`; prefer no MySQL host publish |

### 5.2 Logging, monitoring, and error handling

| Severity | Gap | Recommendation |
|----------|-----|----------------|
| **High** | No health check endpoints | Add `AddHealthChecks` (DB + disk) for reverse proxy and orchestrator |
| **High** | No structured production logging sink | Serilog → file/seq; correlate with request ID |
| **Medium** | Global handler returns 500 with exception text | ProblemDetails + safe messages in production |
| **Medium** | No metrics (Prometheus/OpenTelemetry) | Add basic HTTP and DB timing metrics |
| **Low** | Default ASP.NET logging only | Tune levels; alert on error rate spikes |

### 5.3 Backup system (documented in `Report.md` §7)

| Severity | Finding | Recommendation |
|----------|---------|----------------|
| **Medium** | Backup script not in repo — cannot verify implementation | Commit `backup.sh`; test restore quarterly |
| **Medium** | 7-day local retention only | Off-VPS copy (S3/another server) for ransomware recovery |
| **Low** | Checksum deduplication documented | Good practice; log skipped vs created backups |

### 5.4 Production readiness gaps

| Area | Status |
|------|--------|
| Secrets management | **Fail** — in appsettings and Report |
| Authorization coverage | **Fail** — major dashboard/mobile admin gaps |
| Pagination | **Fail** — catalog and orders |
| Health checks | **Missing** |
| Rate limiting | **Missing** |
| Horizontal scaling (SignalR, files) | **Not ready** |
| Payment webhook security | **Incomplete** (`ProcessPaymentCallback` not implemented) |
| Automated backups | **Documented**, not verified in codebase |

---

## 6. Prioritized Critical Fixes

Execute in this order for maximum risk reduction:

| Priority | ID | Action | Effort |
|----------|-----|--------|--------|
| 1 | S-01, S-02, S-03, S-04 | Lock down admin creation, dashboard mutations, and notification broadcast/send | Low–Medium |
| 2 | S-05, S-06, S-09 | Rotate and externalize all secrets; purge from git history and `Report.md` | Medium |
| 3 | S-07, S-08, S-10 | Fix authentication model; remove `AllowAnonymous` debt IDOR | Medium |
| 4 | P-01, P-02, P-03 | Paginate catalog; fix checkout N+1; split cart query | Medium |
| 5 | DB indexes | Migration for `Favorites.UserId`, `Notifications`, `Orders` | Low |
| 6 | S-13, S-14, S-12 | Disable public Swagger; safe errors; secure SignalR | Low |
| 7 | S-23 | Implement Paymob HMAC callback | Medium |
| 8 | Ops | Add health checks; version Docker stack; verify backup restore | Medium |

---

## Summary

The codebase has a solid EF Core foundation with no raw SQL injection surface, but **authorization gaps and secret handling are production-blocking**. Performance risk is concentrated in **unbounded reads**, **heavy EF includes**, and **checkout N+1**. Database tuning should start with **indexes on `Favorites.UserId` and `Notifications`**. Infrastructure practices described in `Report.md` are directionally sound (localhost binding, backups), but **must be codified in the repository** and hardened (DB user, phpMyAdmin isolation, health checks, secret management).

*Generated from static analysis of the AboKamel solution and deployment documentation.*
