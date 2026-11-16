# CBEUA — Business and Technical Reference

This file is the single source of business context for the Canara Bank retired/serving employees contribution scheme. It covers both repositories:

| Repo | Folder (under `Canara Bank/`) | Role |
|---|---|---|
| `HestiaTechonologyApps/CbeuaApi` | `backend/` (this repo) | ASP.NET Core 8 Web API, EF Core 9, SQL Server |
| `HestiaTechonologyApps/CANARA_BANK` | `CANARA_BANK/` | React 19 + TypeScript + Vite SPA (public, admin and staff portals) |

Read **Working rules** before changing anything. In each workflow section, statements are tagged:
- **[Verified]**: confirmed in source code. File references point to the main implementation.
- **[Gap]**: the business requirement is not met, or is only partly implemented.
- **[Open]**: needs a business decision. Do not invent an answer.

---

## Working rules

- **Branches and deploys**
  - Frontend: `Production` is the live branch (push deploys to SmarterASP). `master` is the working branch (push deploys to Azure Static Web Apps).
  - Backend: `master` only (push deploys to SmarterASP via FTP).
  - Every push to these branches is a production release. Never push, merge or switch branches without explicit approval.
- **Change discipline**
  - Preserve existing working functionality.
  - Make surgical, requirement-driven changes. No broad refactors.
  - Follow the existing layer pattern and reuse existing services, repositories, DTOs, `ApiResponseFactory` and the shared `Kidu*` frontend components.
- **Financial integrity rules**
  - Never bypass contribution validation or approval.
  - Never treat an unresolved discrepancy (unregistered member, unknown branch or circle) as a successful contribution.
  - Prevent duplicate postings. The existing unpark check treats one `Accounts` row per member + month + year as the rule; confirm with the business before relaxing it.
  - Keep traceability from every `Accounts` row back to its source: bank-file detail, direct entry, direct payment, refund or claim.
  - Record who did each sensitive action. Take that identity from the JWT, not from client-supplied IDs.
- **Database**
  - Do not run destructive operations against production.
  - Do not create migrations or schema changes without explicit approval.
  - `appsettings.json` points at the live database. Don't run the API or EF tooling against it casually.
- **Business rules**
  - Don't invent rules. Record unclear rules under **Open questions** and ask.
- **Deferred decision**
  - Plaintext password handling (see Auth) is a known issue. The owner has chosen to keep it for now. Do not change it unless asked.

---

## 1. Business domain

The scheme (Canara Bank Employees' Union Golden Jubilee Family Welfare Scheme, "CBEUGJFWS") is a welfare fund for **serving and retired Canara Bank employees**.

- Members contribute a fixed amount every month, normally by salary/pension deduction reported in a monthly bank file.
- Members can also pay directly when a deduction was missed, request a partial **refund** of their contributions, and on a member's death the nominee receives a **death claim** payout.
- **Org hierarchy:** `Company` → `Circle` (regional unit, `CircleCode`) → `Branch` (`DpCode`, belongs to a circle and state) → `Member` (`BranchId`). `State` is geography and drives refund numbering. `CircleState` is a separate entity.
- **Member** (`Cbeua.Domain/Entities/Member.cs`): `StaffNo` (the bank's employee number and the matching key for bank files), `OldStaffNo`, DOB/DOJ, `DojtoScheme`, designation, category, `StatusId` (a generic `Status` lookup), nominee details (`Nominee`, `NomineeRelation`, `NomineeIDentity`), `IsRegCompleted`, `UnionMember` and `TotalRefund` (both strings), and soft delete `IsDeleted`.
- **User** is the login account (`UserName`, `Role`, `StaffNo`, nullable `MemberId`). Members log in only after self-registration is approved (see §7).
- **Contribution amounts:** the business states members contribute ₹100, ₹150 or ₹200 a month.
  - [Gap] No field on `Member`, `Category` or elsewhere models a member's contribution rate.
  - The bank-file amount is accepted as-is, and direct entries take a free amount.
- **The ledger:** `Accounts` (`Cbeua.Domain/Entities/Accounts.cs`) is the posted contribution ledger. Each row holds `MemeberId` (sic), `BranchId`, `CircleId`, `MonthCode`, `YearOf`, `Amount`, `TransMode`, `Reference` and `Remark`.
  - Refund eligibility and member contribution history are computed from it.
  - Known `TransMode` values [Verified]:

| TransMode | Source | Reference / Remark |
|---|---|---|
| 8 | Monthly bank file: bulk approval, and unpark | `"Bank File"`. Remark is `"Monthly Contribution"` for bulk, `"{masterId}/{detailId}"` for unpark |
| 9 | Account Direct Entry approval | `"Direct Entry"` / `"Account Direct Entry"` |
| 10 | Direct Payment approval | Reference = `ReferenceNo`, Remark = `Remarks` |

  - [Gap] `Accounts.YearOf` is filled inconsistently:
    - Bank-file postings store the **calendar year**, e.g. 2026 (`ContributionMasterRepository.GetDetailsWithLookupsAsync`, `int.Parse(d.Year)`).
    - Direct entries store the **`YearMaster.YearOf` identity**, because the frontend sends `selectedYearMaster.yearOf`.
    - Refund eligibility joins `Accounts.YearOf` to `YearMaster.YearOf`, i.e. it treats the value as the identity.
  - [Open] Which encoding is intended? Check before writing any query on `Accounts.YearOf`.

---

## 2. Monthly contribution processing (bank file)

### Business requirement
1. An authorised user uploads the bank's monthly text file. **One file covers all employees for a month** (confirmed by the business). There is no separate file per circle.
2. The system validates the records and separates valid records from discrepancies.
3. The user reviews the discrepancies, parks the records that can't proceed, and continues with the valid ones.
4. The contribution is submitted for approval. An approver approves or rejects it.
5. Parked records stay available. After the master data is corrected they are processed, without duplicating any posting.
6. Status and audit information are recorded.

### Implementation [Verified]

**Upload and parse.** `POST /api/MonthlyContribution/upload-and-save` (multipart: `ContributionFile`, `MonthCode`, `YearOf` = a `YearMaster` id).
- Controller: `Controllers/MonthlyContributonController.cs` (note the file-name typo). Service: `MonthlyContributionService.UploadAndSaveAsync`.
- The file is saved to `wwwroot/contributionfiles/Contribution_{year}_{month}_{guid}{ext}`. One `MonthlyContribution` row per month/year holds the file metadata.
- **Fixed-width layout.** Lines shorter than 75 characters are rejected as "Wrong Length":

| Chars | Field | Chars | Field |
|---|---|---|---|
| 0–4 | Circle code | 16–21 | StaffNo |
| 5–6 | Month | 22–52 | Name |
| 7–10 | Year | 53–67 | Designation |
| 11–15 | DpCode (branch) | 68–74 | Amount in paise; stored as `int / 100` (the business confirms files carry no paise) |

- Lines whose month/year don't match the selected period ("Wrong Period"), or that fail to parse, are **returned in the response as `ErrorLines`. They are not stored.**
- Each valid line becomes one `ContributionDetail` row (`FullString` keeps the raw line). One `ContributionMaster` is created with:
  - `ContributionStatus = "Uploaded"`, `totalamount`, `totalentry` (stored as strings), `Month` and `Year` (strings, calendar year), and `Circle` (taken from the first line, so it's only a label).
  - `NewMemberCount` = the number of details whose StaffNo is not a member.
  - Details are inserted in batches of 1,000.
- **Re-upload for the same month/year** deletes every existing master and detail for that period **regardless of status** before inserting the new data.
  - [Gap] This includes Approved masters, whose `Accounts` postings remain.
- `PUT /api/MonthlyContribution/{id}/update-contribution` replaces the file and blocks only when the status is `APPROVED`. It resets the status to `"Uploaded"`.
- `DELETE /api/MonthlyContribution/{id}` checks for status `"A"`. [Gap] The real status is `"Approved"`, so the guard never blocks.
- Transactions:
  - [Gap] There is no transaction around delete-old plus insert-new. Several `SaveChanges` calls run in sequence, so a failure can leave the period half replaced.
  - [Gap] The uploader is not recorded: `MonthlyContribution.CreatedByUserId` is not set on upload, and the audit `changedBy` is `"System"`.

**Discrepancy identification.** Discrepancies are **computed at read time, not stored**. Use `GET /api/MonthlyContribution/{masterId}/report?type=…&pageNumber=&pageSize=` (`MonthlyContributionRepository`):

| `type` | Meaning |
|---|---|
| `NEWMEMBERS` | Detail StaffNo has no non-deleted `Member` (employee not registered in the scheme) |
| `WRONGBRANCH` | Detail DpCode matches no non-deleted `Branch` |
| `WRONGCIRCLE` | Detail circle code matches no non-deleted `Circle.CircleCode` |
| `PARKEDITEMS` | Details with `isParked = true` |
| `ALL` | All details of the master |
| `DEFAULTER` | Members with **no** unparked detail for that month/year (missed contribution, likely to need a direct payment) |

- [Gap] The bank does not send "bank information" (account details), and the system has no model for it, so it can't be validated.
- [Gap] Discrepant rows are **not parked automatically**. A user must review the reports and park each row.

**Parking.** `POST /api/ContributionDetail/{detailId}/park` with body `{ parkReason }` sets `isParked`, `ParkReason` (free text typed by the user) and `Parkedon`. `ContributionDetailService.ParkItemAsync`.

**Submit for approval.** `POST /api/ContributionMaster/{id}/forward` is allowed only from `"Uploaded"` and moves the status to `"Forwarded"`. The admin "Approval" list uses `GET /api/ContributionMaster/getall-forwarded-contributions`.

**Approve or reject.** `POST /api/ContributionMaster/{id}/approve?approve=true|false&currentUserId=` is allowed only from `"Forwarded"` (`ContributionMasterService.ApproveAsync`).
- **On approve:**
  - Every **unparked** detail is resolved: Member by StaffNo, Branch by DpCode, Circle by CircleCode.
  - Each resolved detail becomes an `Accounts` row (TransMode 8). Accounts rows and the master update are saved in one `SaveChanges`.
  - The master gets `ContributionStatus = "Approved"`, `isApproved = true`, `ApprovedBy = currentUserId`, and `ApprovedDate` (stored as a string).
- **On reject:** the status becomes `"Rejected"`.
- Approval gaps:
  - [Gap] Unparked rows whose member or branch can't be resolved are **silently skipped** (`ContributionMasterRepository.cs` ~L140, a `TODO`). The master is still marked Approved, so the posted total can be lower than `totalamount`. An unresolved circle is posted as `CircleId = 0`.
    - This breaks the rule "discrepant records must not be silently treated". Any change here must surface or park these rows.
  - [Gap] No duplicate check against `Accounts` and no concurrency token, so two concurrent approvals can post twice.
  - [Gap] `Rejected` is terminal. There is no "return for correction"; the only way back is re-uploading the file.

**Unpark = correct and post.** `POST /api/ContributionDetail/{detailId}/unpark?currentUserId=` (`ContributionDetailService.UnParkItemAsync`).
- It re-resolves member, branch and circle and fails with a reason if any is still missing.
- It refuses if `Accounts` already has a row for that member + year + month.
- Otherwise it **posts directly to `Accounts`** (TransMode 8, Remark `"{masterId}/{detailId}"`) and clears `isParked`.
- [Gap] Unparking does not check the master's status and needs no approval step.
  - If a row is unparked **before** the master is approved, bulk approval will post it **again**, because approval has no duplicate check.
- [Open] Should corrected parked records go through approval?

### Mapping the business states to the data model

| Business concept | Representation today |
|---|---|
| Valid record | `ContributionDetail` with `isParked = false` whose StaffNo, DpCode and circle all resolve. Computed, not stored |
| Discrepant record | Appears in a `NEWMEMBERS`, `WRONGBRANCH` or `WRONGCIRCLE` report. Computed, not stored |
| Parked record | `isParked = true`, `ParkReason`, `Parkedon` |
| Corrected, awaiting reprocessing | **No state.** The master data gets fixed; the detail stays parked until someone unparks it |
| Submitted for approval | Master `ContributionStatus = "Forwarded"` |
| Approved | Master `"Approved"` / `isApproved`. The detail has no approved flag; posting is visible only through `Accounts` (TransMode 8) |
| Rejected | Master `"Rejected"` |

Master status transitions: `Uploaded → Forwarded → Approved | Rejected`. Re-upload resets the period back to `Uploaded`, either by recreating it or through update-contribution.

[Gap] Traceability: rows posted by bulk approval carry no master or detail id. Only unpark postings do, through `Remark`.

---

## 3. Direct contributions (missed deductions paid directly)

Purpose: record contributions that did not arrive through the bank file. There are **two parallel implementations** [Verified]. [Open] Which one is authoritative, and are both used?

**Account Direct Entry** (`AccountsDirectEntry`; controller `AccountDirecyEntryController`, typo kept; route `/api/AccountDirecyEntry`)
- Fields: `MemberId`, `BranchId`, `MonthCode`, `YearOf` (the YearMaster id), `DdIba` / `DdIbaDate` (demand draft / IBA reference), `Amt`, legacy form columns `Enrl` / `Fine` / `F9`–`F11`, `status` (the staff portal defaults it to `"Submitted"`), `isApproved`, `ApprovedBy`, `ApprovedDate`.
- Created from the admin portal (any member) and the staff portal (the logged-in member's own entries).
- `PUT approve/{id}?approve=&currentUserId=`:
  - Approve posts `Accounts` TransMode 9 (circle taken from the branch) and sets `status = "Approved"`.
  - Reject sets `"Rejected"`.
- Create, update, delete and approve are audited.
- Gaps:
  - [Gap] Only `isApproved` is checked, so a Rejected entry can later be approved.
  - [Gap] No duplicate check against `Accounts` for the same member and month.

**Direct Payment** (`DirectPayment`; `/api/DirectPayment`)
- Fields: `MemberId`, `Amount`, `PaymentDate`, `PaymentMode` (Cash/Bank/UPI per a code comment), `ReferenceNo`, `Remarks`, `isApproved`, `ApprovedBy`, `ApprovedDate`. There is no status field.
- Approval posts `Accounts` TransMode 10. **The posted period is the payment date's month/year, not the missed month.**
- [Gap] A rejection only sets `ApprovedBy` and `ApprovedDate`, so it is indistinguishable from pending.
- [Gap] No duplicate check.
- [Gap] The controller has **no `[Authorize]`**, so it is anonymously reachable.

There is no payment gateway; all payments are recorded manually. Neither flow validates the amount against a contribution rate.

---

## 4. Refunds

Purpose: a member withdraws part of their accumulated contributions. This is a separate workflow from death claims.

[Verified] `RefundContribution` entity and `/api/RefundContribution`:
- **Fields:** `MemberId`, `StateId`, `DesignationId`, `RefundNO`, `BranchNameOFTime` / `DPCODEOfTime` (a snapshot at the time of the request), `Type` (frontend options: `Refund`, `Loan`, `Emergency`), `Remark`, `DDNO`, `DDDATE`, `Amount`, `LastContribution`, `YearOF`, `isApproved`, `ApprovedBy`, `ApprovedDate`.
- **Who creates them:** the admin portal (any member) and the staff portal (own refunds only, identified by `memberId` stored in the browser).
- **Eligibility** (`RefundContributionRepository.GetMemberRefundEligibilityAsync`, exposed as `GET member-eligibility/{memberId}`):
  - `Available = Σ Accounts.Amount for the member − Σ approved refunds − Σ non-approved refunds`, with soft-deleted refunds excluded.
  - It also returns the last contribution month, year and amount.
  - Create and update reject amounts above `Available`.
- **Refund number:** `GET next-refund-no/{stateId}` returns the state code plus (highest existing numeric suffix + 1). [Gap] There's no uniqueness constraint, so concurrent creates can clash.
- **Approval** (`PUT approve/{id}`) only sets the flag, approver and date. It does not re-check eligibility, writes nothing to `Accounts`, and has no paid or completed state; DD number and date are just fields.
- Gaps:
  - [Gap] A rejection leaves `isApproved = false`, so a **rejected refund keeps counting as "pending"** and permanently reduces the member's available balance.
  - [Gap] Refunds are not linked to the specific `Accounts` rows they draw down.
- [Open] Refund eligibility rules (minimum membership period, maximum percentage, frequency), and what `Loan` and `Emergency` types mean.

---

## 5. Death claims and nominees

Purpose: when a member dies, the nominee receives the scheme benefit. This is distinct from refunds.

[Verified] `DeathClaim` entity and `/api/DeathClaim`:
- **Fields:** `MemberId`, `StateId`, `DesignationId`, `DeathDate`, `Nominee`, `NomineeRelation`, `NomineeIDentity`, `DDNO` / `DDDATE` (payout instrument), `Amount`, `LastContribution`, `YearOF`, `isApproved`, `ApprovedBy`, `ApprovedDate`.
- Nominee data lives on `Member` as free text. It is copied onto the claim; there is no separate nominee entity.
- **Path B (organisation-initiated)** is implemented: admin-portal claim create, edit, view and approve screens. Claim screens include the generic attachment component (the `Attachment` table, keyed by table name and record id), but the backend enforces no mandatory documents.
- **Path A (nominee-initiated)** is **not implemented**. Nominees have no login and there is no public claim form. [Open] Is it required?
- **Approval** only sets the flag, approver and date. No `Accounts` posting and no paid or completed state.
- The public portal's **Claims page** lists all death claims through the anonymous `GET /api/DeathClaim`, together with `ClaimsSettled/claims-settled-stats`. This publication appears intentional, but it exposes nominee identity details.
- Gaps:
  - [Gap] No check that a member has only one claim.
  - [Gap] The member is not marked deceased, and contributions or defaulter reports are not stopped.
  - [Gap] The payable amount is entered manually; there's no formula.
  - [Gap] A rejection is indistinguishable from pending.
- [Open] How should a deceased member be represented (a `StatusId` value?), the benefit formula, and the required documents.

---

## 6. Other financial and admin modules

- **Expense Master / Expense Type:** the association's expenses, with an approve flow (`ExpenseMaster.isApproved`).
- **Dashboard:** `/api/Dashboard/*` gives overview figures, contributions vs claims, state-wise membership and trends.
- **Reports:** `Report`, `ReportType` and `ReportEngine` (`ReportEngine.SQLString` is stored but never executed).
- **CMS / public site:** `PublicPage`, `DailyNews`, `DayQuote`, `ManagingComitee`, `ContactMessage`, and `Public*` endpoints (anonymous).
- **Support tickets:** `SupportTicket`, with an auto-generated ticket number (`next-ticket-num`).
- **HRM module:** the association's own HR (employees, recruitment, leave). It's separate from members and has no frontend screens.

---

## 7. Roles, authentication and authorization

- **Roles** are free-text `User.Role` values: `SystemAdmin` and `Administrator` use the admin portal (`/dashboard`); `OfficeStaff` and `DEO` use the staff portal (`/staff-portal`).
  - The staff portal is in practice **member self-service**: profile, own direct entries, own refunds and contribution history.
  - Self-registration (`POST /api/Auth/register`) needs an existing `Member.StaffNo` and creates a `UserRegistration` with Role `"OfficeStaff"`. An admin approves it (`POST /api/UserRegistration/{id}/approve`), which creates the `User`.
- **Authentication:** JWT, 7-day expiry, signed with `Jwt:Key`; the issuer value is also used as audience. Claims: `NameIdentifier` = **UserName** (not the id), `SerialNumber` = UserId, `Role`, `Email`, `GroupSid` = CompanyId, `MobilePhone`.
  - `AuthController.change-password` and `me` call `int.Parse(NameIdentifier)`, which is wrong; use `SerialNumber` for the user id.
- **Authorization as implemented:**
  - [Gap] There are **no server-side role checks**: no `[Authorize(Roles)]`, no policies, no fallback policy. `UserRoleRight` is stored but enforced nowhere. Role gating exists only in the frontend `ProtectedRoute`, which reads `localStorage`.
  - [Gap] Some controllers have no `[Authorize]` at all: `DirectPayment`, `Comment`, `MemberAccountsDetails`, `PublicPage` (including writes), and most HRM controllers.
  - [Gap] There are no ownership checks, so any logged-in user can read or edit any member's records by id.
  - [Gap] **No maker-checker.** The same user can upload, forward and approve. Approver identity is the client-supplied `currentUserId` query parameter.
  - [Open] Which roles may upload, forward and approve contributions and claims, and is a separate checker required?
- **Passwords:** a BCrypt helper exists, but admin-created users and `POST /api/User/ChangePassWord` store plaintext, and `PasswordHelper.VerifyPassword` accepts `password == storedValue`. **Deferred by decision; do not change unless asked.**
- **Forgot password:** non-functional. The reset tokens live in a dictionary on a scoped service and are lost after the request, and the frontend has no reset screen.

## 8. Audit and logging

- **`AuditLog`** (`Entities/Common/AuditLog.cs`): `TableName`, `Action`, `RecordID`, `ChangedBy`, `ChangedAt`, `ChangeDetails`. It's written by `AuditRepository.LogAuditAsync<T>` and read through `GET /api/AuditLog/{table}/{recordId}` (the frontend `KiduAuditLogs` component).
  - Covered: CRUD plus approve for direct entries, direct payments, refunds, death claims and expenses, and contribution upload.
  - [Gap] Contribution forward, approve, park and unpark are **not** audited (`ContributionMasterService` and `ContributionDetailService` have 0 calls).
  - [Gap] `changedBy` is often `"System"`. A parallel `AuditTrailController` also exists.
- **Serilog** writes to the console and to `logs/info_log.txt` and `logs/error_log.txt`.
- **`ExceptionLoggingMiddleware`** returns a 500 response with the exception message. Persisting the logs to the database is commented out.

---

## 9. Backend technical reference

### Solution layout
```
Cbeua.Api        → Controllers, Program.cs, Middlewares, wwwroot (uploads), appsettings
Cbeua.Bussiness  → Services (business logic), DI registration (BussinessServiceCollectionExtensions)
Cbeua.Core       → AppDbContext, EF Migrations, Repositories, Helpers, DI (InfraCoreServiceCollectionExtensions)
Cbeua.Domain     → Entities, DTOs, Interfaces (IServices/IRepositories), Constants, Configurations
```
Dependency flow: `Controller → I{X}Service → {X}Service → I{X}Repository → {X}Repository → AppDbContext → SQL Server`.

- **New entities** need 6 files: `Entities/{X}.cs`, `DTO/{X}DTO.cs`, `I{X}Service` + `{X}Service`, `I{X}Repository` + `{X}Repository`, and `{X}Controller`.
  - Register them in **both** DI extension methods; everything is `AddScoped`, with no assembly scanning.
- `GenericRepository<T>` provides CRUD plus a reflection-based `SoftDelete`.
- There are no tests.

### API conventions
- Route `api/[controller]`, no versioning. `CompanyController` and `FinancialYearController` use `api/[controller]/[action]`.
- **Responses:** most actions return `CustomApiResponse { StatusCode, Error, CustomMessage, IsSucess (sic), Value }`.
  - Prefer `ApiResponseFactory.Success/Fail` (`Cbeua.Core/Helpers`) for new code.
  - The HTTP status is often 200 even when the body's `StatusCode` reports an error; the frontend checks `isSucess`.
  - Some controllers (`ContributionMaster`, parts of `MonthlyContribution`) return `IActionResult` instead.
- **Pagination** is per controller (for example `Member/paged`, `RefundContribution/paged`). Don't extend `Api_PaginatedListDataController`, which is legacy.
- **Approve endpoints** share the shape `approve/{id}?approve=bool&currentUserId=int`.
- **Uploads:**
  - `[Consumes("multipart/form-data")]` + `[FromForm]`, saved under `wwwroot/{profilepics,companylogos,committeeimages,contributionfiles,uploads/...}`.
  - These folders are publicly served by `UseStaticFiles`.
  - No extension allowlist. `Attachment` upload builds its path from an unvalidated `TableName`.

### Persistence
- EF Core code-first with data annotations only. `OnModelCreating` is empty: no fluent config, indexes, query filters or concurrency tokens.
- **Soft delete** is a manual `IsDeleted` filter in every query.
- **Keys and types to watch:**
  - `ContributionMaster.Month` and `Year` are **strings** (calendar year).
  - `ContributionDetail.StaffNo`, `DpCode`, `Month` and `Year` are **strings**; `Member.StaffNo` and `Branch.DpCode` are **ints**. Joins use `.ToString()`, so they're non-sargable.
  - `MonthlyContribution.YearOf` and `AccountsDirectEntry.YearOf` are `YearMaster` ids. See the `Accounts.YearOf` gap in §1.
- `MonthlyContribution` (one row per uploaded month: the file record) and `ContributionMaster`/`ContributionDetail` (parsed data) **both exist and are both used** by the upload flow.
- No explicit transactions; no raw SQL.

### Configuration and hosting
- `appsettings.json` holds the live connection string and JWT key in plain text. Don't copy them anywhere, and rotate them if they're exposed.
- `OtpSettings` and `Wallet` are bound from configuration.
- CORS allows any origin. Swagger is always on. HTTPS redirection is disabled (IIS on SmarterASP terminates TLS).
- Nothing sits in front of the API (no WAF or IP allowlist), so anonymous endpoints are internet-exposed.

### Naming quirks (search for the misspelled form)
- `Bussiness`, with occasional `Cbeua.Business` namespaces.
- `IsSucess`.
- `MemeberId` (on `Accounts` only).
- `ManagingComitee`, `ComapanyName`, `AccountDirecyEntry`, `MonthlyContributonController.cs`.
- `HRMSJobCategoriess` and `HRMSJobCategorys`: two DbSets on one entity.
- `Cbeua.InfraCore` and `Cbeua.CORE` namespaces.
- `Member.GenderId`: 0 = Male, 1 = Female, anything else = Others.

---

## 10. Frontend technical reference (`CANARA_BANK/`)

- **Stack:** React 19, react-router-dom 7, react-bootstrap, react-hot-toast, sweetalert2, recharts, jspdf and xlsx. There's no state library and no React Query.
- **Entry:** `src/main.tsx` (with `YearProvider`) → `App.tsx` → `src/Routes/AppRoutes.tsx`, which combines `PUBLIC-PORTAL/Routes/Route.tsx`, `ADMIN-PORTAL/Routes/Route.tsx` (under `/dashboard`) and `STAFF-PORTAL/Routes/Routes.tsx` (under `/staff-portal`).
- **Folder pattern** per portal: `Pages/<Module>/{List,Create,Edit,View}.tsx`, `Services/<Module>.services.ts` (static classes) and `Types/<Module>.types.ts`.
  - Shared building blocks live in `src/Components/Kidu*`: `KiduServerTable` (paged lists), `KiduCreate` / `KiduEdit` (form generators driven by a field-rules array), `KiduAttachments`, `KiduAuditLogs`, `KiduValidation`, and others.
  - Reuse them rather than writing new form or table code.
- **API layer:**
  - Every URL is defined in `src/CONSTANTS/API_ENDPOINTS.ts`. The base is `VITE_API_BASE_URL`, falling back to `https://api.cbeugjfws.co.in/api`.
  - Calls go through `HttpService.callApi` (`fetch` plus a Bearer token from `localStorage.jwt_token`).
  - A few uploads call `fetch` directly.
  - `MonthlyContribution.services.ts` (the `MONTHLY_CONTRIBUTION` group) is dead code; the live flow uses `MonthlyContributionMasters.services.ts`.
- **Auth state:** `localStorage` keys `jwt_token`, `user`, `user_role`, `member_id`, `staff_no` and `token_expires_at` (`src/Services/Auth.services.ts`, which logs verbosely to the console). Route guard: `PUBLIC-PORTAL/Auth/ProtectedRoute.tsx`.
- **Contribution screens:**
  - Upload and review: `ADMIN-PORTAL/Pages/Contributions/MonthlyContribution/*`. `View.tsx` has inline `MemberCreateModal` / `BranchCreateModal` / `CircleCreateModal`, prefilled from a discrepant detail, for fixing master data. The standalone `CreateMember/Branch/Circle.tsx` pages in that folder aren't routed or imported.
  - Approval: `ADMIN-PORTAL/Pages/Approval/*`.
- **Version:** bump `src/CONSTANTS/Version.ts` on release.
- **Builds:**
  - Modes: `npm run build | build-admin | build-public | build-staff`.
  - `npm run build` runs plain `tsc` against a root `tsconfig.json` with `files: []`, so it does **not** type-check. Use `npx tsc --noEmit -p tsconfig.app.json`.
  - ESLint currently reports about 400 problems.

---

## 11. Open questions (need business answers)

1. Which roles may upload, forward and approve contributions, direct entries, refunds and claims? Is a separate checker (maker-checker) required?
2. Should an approved bank-file batch ever be replaceable? If so, how should its `Accounts` postings be reversed?
3. Should unparked (corrected) records need approval before posting? How should rows that are still unresolved at approval be handled: block approval, auto-park, or report?
4. Should a rejected contribution be returnable for correction?
5. Is `Accounts.YearOf` meant to hold the calendar year or the `YearMaster` id?
6. Account Direct Entry vs Direct Payment: is one of them obsolete? For Direct Payment, which month should be credited?
7. How should each member's contribution rate (₹100 / ₹150 / ₹200) be modelled and validated?
8. Refund rules: eligibility, limits, frequency, the meaning of the `Loan` and `Emergency` types, and whether a refund reduces future eligibility only or the ledger itself.
9. Death claims: is a nominee-initiated path needed? How is a member marked deceased? What are the benefit formula and the mandatory documents? Should the public claims listing show nominee identity?
10. What "bank information" discrepancy should be detected? The file and the data model currently carry none.
