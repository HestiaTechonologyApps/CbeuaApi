# CBEUA Backend — Reference Guide

Backend API for the **Canara Bank Employees Union / Association** (CBEUA) app. ASP.NET Core Web API (.NET) with a layered architecture, EF Core (SQL Server), JWT auth. Serves member management, monthly union contributions, direct-entry accounting, death claims, support tickets, public-facing content, and an embedded HRM (HR management) module.

## Solution layout

```
Cbeua.Api          → Web API host: Controllers, Program.cs, Middlewares, wwwroot (uploads), appsettings
Cbeua.Bussiness     → Service layer: business logic, one *Service per entity, DI registration (BussinessServiceCollectionExtensions)
Cbeua.Core          → Infrastructure: AppDbContext, EF Migrations, Repositories, Helpers, DI registration (InfraCoreServiceCollectionExtensions)
Cbeua.Domain        → Cross-cutting: Entities (EF models), DTOs, Interfaces (IServices/IRepositories), Constants, Configurations
```

Note the misspelling `Bussiness` (not "Business") is baked into the namespace/folder name — some files also reference `Cbeua.Business.Services` (singular typo-fix) inconsistently; both exist in `using` statements.

Layer dependency flow (strict, one direction):

```
Controller  →  I{Entity}Service (Cbeua.Domain.Interfaces.IServices)
            →  {Entity}Service   (Cbeua.Bussiness.Services)
            →  I{Entity}Repository (Cbeua.Domain.Interfaces.IRepositories)
            →  {Entity}Repository  (Cbeua.Core.Repositories, namespace Cbeua.InfraCore / Cbeua.CORE)
            →  AppDbContext (Cbeua.Core.Data, namespace Cbeua.InfraCore.Data)
            →  SQL Server
```

Almost every domain entity gets this same four-file treatment: `Entities/{X}.cs`, `DTO/{X}DTO.cs`, `Interfaces/IServices/I{X}Service.cs` + `Bussiness/Services/{X}Service.cs`, `Interfaces/IRepositories/I{X}Repository.cs` + `Core/Repositories/{X}Repository.cs`, and `Api/Controllers/{X}Controller.cs`. When adding a new entity, follow this exact set of 6 files.

A `GenericRepository<T> : IGenericRepository<T>` (Cbeua.Core/Repositories/GenericRepository.cs) provides generic CRUD (`GetAllAsync`, `GetByIdAsync`, `AddAsync`, `Update`, `Delete`, `SoftDelete` via reflection on an `IsDeleted` bool property, `FindAsync`, `AnyAsync`, `CountAsync`, `SaveChangesAsync`) and is injected as `IGenericRepository<>` — some entity-specific repositories extend/wrap this instead of hand-rolling CRUD.

## DI registration

Two extension methods wire everything up, called from `Program.cs`:
- `builder.Services.AddInfraCoreServiceCollectionExtensions(builder.Configuration)` — registers `AppDbContext` (SQL Server via `DefaultConnection`), every `I{X}Repository → {X}Repository`, and `IGenericRepository<>`.
- `builder.Services.AddApplicationServices(builder.Configuration)` — registers every `I{X}Service → {X}Service`.

Everything is `AddScoped`. New entities must be added to **both** extension methods manually (no assembly scanning) — this is the most common thing to forget when adding a module.

## API conventions

- Route pattern: `[Route("api/[controller]")]` on nearly every controller (i.e. `/api/Member`, `/api/Auth`, `/api/ContributionMaster`, etc.) — controller name = route segment, no API versioning.
- Most controllers are `[Authorize]` by default (JWT bearer); a handful of read/public ones (`PublicController`, `PublicPageController`, `ContactPageController`, `DailyNewsController`) are open, and `AuthController` login/register/forgot/reset endpoints are anonymous by omission.
- Every action returns `CustomApiResponse` (Cbeua.Domain/DTO/CustomApiResponse.cs) — not standard `IActionResult`/`ProblemDetails`:
  ```csharp
  public class CustomApiResponse {
      public int StatusCode { get; set; }
      public string Error { get; set; }
      public string CustomMessage { get; set; }
      public bool IsSucess { get; set; }   // note: "IsSucess" typo, not "IsSuccess" — used everywhere
      public object Value { get; set; }
  }
  ```
  Two construction styles coexist in the codebase:
  1. **Older/manual style** (most CRUD controllers, e.g. `MemberController`): build `new CustomApiResponse()` inline in each action, try/catch around service calls, `response.StatusCode` set by hand (200/201/204/400/404/500).
  2. **Newer style** (`AuthController` and some others): `ApiResponseFactory.Success(data, message, HttpStatusCode)` / `ApiResponseFactory.Fail(message, HttpStatusCode, error)` (Cbeua.Core/Helpers/ApiResponseFactory.cs) — prefer this style for new controllers, it's more consistent.
  - HTTP status codes are always echoed into the JSON body's `StatusCode` field, and the actual HTTP response status is **also** set (framework returns 200 by default for `Task<CustomApiResponse>` unless explicitly wrapped — check individual actions).
- Pagination: ad hoc per-controller, not a single generic mechanism. See `MemberController.GetPagedMembers` (`MemberPaginationParams`) and `Api_PaginatedListDataController` (`/api/Api_PaginatedListData/trips-paginated`, `PaginationParameterDTO` with `pagesize`/`pagenumber`/`filtertext`) — the latter looks like a legacy/in-progress generic list endpoint (mostly commented out), don't extend it, prefer per-controller paged endpoints like Member's.
- `Api_BaseController` is a near-empty base (`CurrentUserID`, `EntityName` fields, unused in most controllers) — most controllers actually inherit `ControllerBase` directly instead.
- File uploads use `[Consumes("multipart/form-data")]` + `[FromForm]` DTOs (e.g. `MemberController.UploadProfilePic`), saved under `Cbeua.Api/wwwroot/{profilepics,companylogos,committeeimages,contributionfiles}` and referenced back as `/relativepath` strings on the entity (e.g. `Member.ProfileImageSrc`).

## Auth flow

- JWT bearer, configured in `Program.cs` via `AddAuthentication().AddJwtBearer(...)`, validated against `Jwt:Issuer` / `Jwt:Key` in appsettings (issuer is reused as audience — same value for both).
- `AuthController` (`/api/Auth`): `login`, `register`, `forgot-password`, `reset-password`, `change-password` (`[Authorize]`), `me` (`[Authorize]`), `logout` (client-side token discard only, no blacklist).
- `JwtService.GenerateToken(UserDTO)` issues claims: `NameIdentifier` = **UserName** (not numeric UserId — see gotcha below), `MobilePhone`, `Email`, `GroupSid` = CompanyId, `Role`, `SerialNumber` = UserId. Token expires in 7 days.
- **Gotcha:** `AuthController` reads `User.FindFirst(ClaimTypes.NameIdentifier)` and does `int.Parse(...)` expecting a numeric user id, but `JwtService` puts `UserName` (a string) into `NameIdentifier` and the numeric `UserId` into `SerialNumber`. Verify which claim is actually the source of truth before relying on `GetCurrentUser`/`ChangePassword` user-id resolution when touching auth code.
- Roles are free-text strings on `User.Role` (not ASP.NET Identity roles) — no `[Authorize(Roles=...)]` usage observed; authorization is mostly all-or-nothing `[Authorize]` plus manual checks in service/business logic.
- Passwords are hashed with BCrypt (`Cbeua.Bussiness/Helpers/PasswordHelper.cs`), **but** `VerifyPassword` first checks `password == hashedPassword` (plain equality) before falling back to `BCrypt.Verify` — see security note below.
- `Member.GenderId` convention: `0` = Male, `1` = Female, anything else = "Others" (mapped in `MemberRepository`'s DTO projection, not stored as a lookup table).
- `ValidationStatusCode` enum (Cbeua.Domain/Constants/ClsEnumCommon.cs) covers OTP-related states (`Success`, `InvalidOtp`, `OtpExpired`, `AccountLocked`, `TooManyAttempts`, `MobileNotRegistered`, etc.) — OTP settings live in `appsettings.json` under `OtpSettings` (expiry minutes, max wrong attempts, resend cooldown).

## Domain model — key entities & nomenclature

**Org hierarchy:** `Company` → `Circle` (regional unit, has `CircleCode`, `Abbreviation`, `StateId`, active date range) → `Branch` (has `DpCode`, belongs to a `CircleId`/`StateId`) → `Member` (belongs to a `BranchId`). `State` is a separate geography lookup, not to be confused with `CircleState` (a different entity — looks like circle-level state/status tracking, not Indian states).

**Member / User split:** `Member` is the union member record (staff no, DOB/DOJ, designation, category, nominee details, profile pic, `IsRegCompleted`, soft-delete `IsDeleted`). `User` is the login/account record (`UserName`, `UserEmail`, `PasswordHash`, `Role`, `StaffNo`, optional `MemberId` FK, `CompanyId`, lock/active flags). A Member doesn't necessarily have a User (registration must complete first — see `IsRegCompleted`), and `User.MemberId` is nullable.

**Contributions** (monthly union dues, the app's core financial workflow):
- `ContributionMaster` — one row per **uploaded batch file** (e.g. a monthly circle contribution file): `FileName`/`FileLocation`/`FileType`/`FileExtension`/`FileSize`, `Month`/`Year`/`Circle` (all stored as strings, not FKs — watch for this when joining/filtering), aggregate `totalamount`/`totalentry`/`NewMemberCount`, `ContributionStatus`, `isApproved`/`ApprovedBy`/`ApprovedDate`. Status workflow: **DRAFT → FORWARDED → APPROVED** (string values on `ContributionStatus`) — `ContributionMasterController` exposes this as separate endpoints (`getall-forwarded-contributions`, `{masterId}/forward`, `{masterId}/approve`, `{masterId}/parked` for paginated parked-entry review); delete is blocked once a master is APPROVED.
- `ContributionDetail` — one row per **line item** within a master file (per-member contribution for that month): `FullString` (raw parsed line), `DpCode`, `StaffNo`, `Name`, `Designation`, `Amount`, `isParked`/`ParkReason`/`Parkedon`/`UnParkedon` (a park/unpark workflow for holding back suspect entries before approval), FK `ContributionMasterId`.
- `MonthlyContribution` — a separate, more normalized monthly contribution entity (added later per migration history) — check which of `ContributionDetail` vs `MonthlyContribution` is authoritative before extending either; they appear to overlap in purpose.
- **Approval nomenclature**: "parked" = held/flagged pending review (detail-level), "approved" = signed off (master/detail and direct-entry level) — both patterns recur across `ContributionMaster`, `AccountsDirectEntry`, and reports.

**Accounts (payments):**
- `Accounts` — a posted contribution payment record: `CircleId`/`BranchId`/`MemeberId` (sic — typo'd FK name, not "MemberId"), `MonthCode`/`YearOf` (int-coded, not string), `Amount`, `TransMode`, `Reference`, `Remark`.
- `AccountsDirectEntry` — manual/direct entry of a member's monthly payment (as opposed to a bulk `ContributionMaster` file upload): `MemberId`, `BranchId`, `MonthCode`/`YearOf`, `DdIba`/`DdIbaDate` (demand-draft/IBA reference), `Amt`, `Enrl`/`Fine`/`F9`/`F10`/`F11` (legacy/report-column-shaped fields, likely mirrors a paper form), `status`, `isApproved`/`ApprovedBy`/`ApprovedDate`, soft-delete `IsDeleted`. `AccountDirectEntryController` + `AccountDirectEntryService` handle create/approve flows for this.
- `RefundContribution` and `DeathClaim` are related financial workflows (member refunds and death-claim payouts) — separate entities/controllers, not sub-types of Accounts.

**Lookups/reference data:** `Status` (generic name/abbreviation/description/group — reused across multiple entities' status fields rather than per-entity enums), `Category`, `Designation`, `Month`, `YearMaster`, `FinancialYear`, `UserType` (abbreviation/description), `UserRoleRight` (permission matrix per role), `CircleState`.

**Content/CMS-ish entities:** `MainPage`, `PublicPage`, `DailyNews`, `DayQuote`, `ContactPage`/`ContactMessage`, `ManagingComitee` (sic — "Comitee" typo used consistently), `Company` (site branding: logo, invoice prefix, contact info) — these back the public-facing website content, served via `PublicController`/`PublicPageController`/`MainPageController` (mostly anonymous access).

**Cross-cutting/common entities** (`Entities/Common/`): `Attachment` (generic file attachment, likely polymorphic via an entity-type/id pair), `AuditLog`, `Comment`, `ExceptionLog`, `FinancialYear`.

**Reporting:** `Report`, `ReportType`, `ReportEngine` + matching services/controllers — a configurable report-generation subsystem (`ReportEngineController`/`ReportEngineService`) separate from ad hoc per-controller list endpoints.

## Naming quirks to know before searching the codebase

These are real, consistent misspellings/naming choices in the code — search for the misspelled form, not the "correct" one:
- `Bussiness` (project/namespace), occasionally `Cbeua.Business` in `using` statements (inconsistent, both resolve to the same assembly's types via using aliases/duplication).
- `IsSucess` (not `IsSuccess`) on `CustomApiResponse`.
- `MemeberId` (not `MemberId`) on `Accounts` entity only — `AccountsDirectEntry`/`Member`/`User` all correctly use `MemberId`.
- `ManagingComitee` (not `Committee`) — entity, DTO, repository, service, controller all use this spelling.
- `ComapanyName` (not `CompanyName`) on `Company` entity.
- `HRMSJobCategoriess` DbSet (double "s" typo) exists alongside `HRMSJobCategorys` DbSet on the *same* `HRMSJobCategory` entity in `AppDbContext` — two DbSet properties mapped to one entity; check which one is actually used before adding queries against job categories.
- Namespace `Cbeua.InfraCore` / `Cbeua.CORE` (mixed casing) both appear for the Core project's repository namespace.

## Cross-cutting concerns

- **Global exception handling**: `ExceptionLoggingMiddleware` (Cbeua.Api/Middlewares) wraps the whole pipeline, catches unhandled exceptions, builds an `ExceptionLog` entity (path, method, query, user, headers, exception details, trace id) — but the actual DB save (`exceptionLogService.LogExceptionAsync`) is **commented out**, so exception logs are currently not persisted; it just returns a 500 JSON body with the raw exception message (a minor info-leak — worth tightening if this becomes internet-facing beyond current use).
- **Soft delete**: convention is a plain `bool IsDeleted` property per entity (not a global EF query filter — `AppDbContext.OnModelCreating` is empty, no `HasQueryFilter` applied anywhere), so every repository/service must manually filter `IsDeleted == false`; `GenericRepository.SoftDelete` sets it via reflection.
- **Audit logging**: `AuditLog`/`AuditLogService`/`AuditLogController` + a parallel `AuditTrailController` — two separate audit-related controllers exist; confirm which is live before adding new audit hooks.
- **Serilog**: configured in `Program.cs` + `appsettings.json` (`Serilog` section) — writes to console and rolling files `logs/info_log.txt` / `logs/error_log.txt`, enriched with machine name/thread id.
- **CORS**: wide open (`AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()`) — fine for current mobile/web client setup but revisit before hardening.
- **Swagger**: enabled unconditionally (not gated to Development) at `/swagger`, with JWT bearer security definition wired in so tokens can be tested directly from the Swagger UI.
- **HTTPS redirection is disabled** (`app.UseHttpsRedirection()` commented out in Program.cs) — likely because hosting is behind IIS/a reverse proxy handling TLS (there's a `web.config`, suggesting IIS/Windows hosting or an Azure App Service model).

## HRM module

A largely self-contained HR module lives under `HRM`/`HRMS` naming, mirroring the main layered pattern but in its own sub-namespaces (`*.HRMS`, folders `Controllers/HRM`, `Services/HRMS` (business), `Repositories/HRMS` (core), `Interfaces/*.HRMS`, `Entities/HRMS/HrmsEntities.cs` — note entities are consolidated into a single file here rather than one-file-per-entity like the rest of the domain).

Covers: `HRMBranch`, `HRMDepartment`, `HRMDesignation`, `HRMDocumentType`, `HRMAwardType`, `HRMEmployee` + `HrmEmployeeAward`, and a recruitment sub-flow (`HRMSJobCategory`, `HRMSJobType`, `HRMSJob`, `HRMSJobLocation`, `HRMSInterviewType`, `HRMSCandidateSource`, `HRMSCandidate`), plus leave management (`HRMSLeaveType`, `HRMSLeaveApplication` — the most recent migrations in the repo, `AddLeaveTypeAndLeaveApplication` / `SyncLeaveApplicationSchema`, so this is actively evolving). This module is functionally separate from the union/member/contribution domain — it's HR-for-the-association's-own-staff, not member management.

## Database & migrations

- EF Core Code-First, SQL Server, migrations under `Cbeua.Core/Migrations` — migration history (Dec 2025 → Mar 2026) shows the schema evolved incrementally per feature (e.g. `addedhrms`, `renamedHRMS_Branch`, `AddLeaveTypeAndLeaveApplication`) rather than being designed upfront; expect ongoing schema churn especially in the HRM module.
- No global query filters or explicit indexes/fluent config in `OnModelCreating` — all mapping is via data annotations on entities (`[Key]`, `[DatabaseGenerated]`) plus EF Core conventions.

## ⚠️ Security note

`Cbeua.Api/appsettings.json` currently has a **live production SQL Server connection string with a real username/password** committed in plain text (`ConnectionStrings:DefaultConnection`), and `Jwt:Key` (the token signing secret) is also a plain hardcoded string in the same file. If this repo is or becomes shared/public, both should move to a secrets manager (User Secrets locally, environment variables / Azure Key Vault / App Service configuration in production) and the exposed DB password should be rotated.

Additionally, `PasswordHelper.VerifyPassword` (`Cbeua.Bussiness/Helpers/PasswordHelper.cs`) accepts a password if it exactly equals the stored `PasswordHash` string *before* attempting BCrypt verification. This looks like a legacy-plaintext-migration shim, but as written it means any account whose stored hash a user could guess/observe (or any legacy row still holding a plaintext password) can be logged into without going through BCrypt at all. Worth confirming no rows in the `Users` table actually still hold plaintext passwords, and removing the shortcut once migrated.
