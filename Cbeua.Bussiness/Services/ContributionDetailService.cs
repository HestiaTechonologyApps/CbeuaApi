using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class ContributionDetailService : IContributionDetailService
    {
        private readonly IContributionDetailRepository _repo;

        public ContributionDetailService(IContributionDetailRepository repo)
        {
            _repo = repo;
        }

        public async Task<CustomApiResponse> GetByIdAsync(long detailId)
        {
            try
            {
                var detail = await _repo.GetByIdAsync(detailId);
                if (detail == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution detail not found",
                        StatusCode = 404
                    };

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = detail
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message}",
                    StatusCode = 500
                };
            }
        }

        public async Task<CustomApiResponse> ParkItemAsync(long detailId, string parkReason)
        {
            try
            {
                var detail = await _repo.GetByIdAsync(detailId);
                if (detail == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution detail not found",
                        StatusCode = 404
                    };

                if (detail.isParked)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Item is already parked",
                        StatusCode = 400
                    };

                var notOpen = await EnsureOpenForParkingAsync(detail.ContributionMasterId);
                if (notOpen != null) return notOpen;

                detail.isParked = true;
                detail.ParkReason = parkReason;
                detail.Parkedon = DateTime.Now;

                _repo.Update(detail);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = "Item parked successfully"
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message}",
                    StatusCode = 500
                };
            }
        }

        // ── Park many specific rows at once ───────────────────────────────
        public async Task<CustomApiResponse> ParkItemsAsync(long masterId, List<long> detailIds, string parkReason)
        {
            try
            {
                if (detailIds == null || detailIds.Count == 0)
                    return Fail("Select at least one item to park.", 400);
                if (string.IsNullOrWhiteSpace(parkReason))
                    return Fail("Please provide a reason for parking.", 400);

                var notOpen = await EnsureOpenForParkingAsync(masterId);
                if (notOpen != null) return notOpen;

                int requested = detailIds.Distinct().Count();
                int parked = await _repo.ParkDetailsAsync(masterId, detailIds, parkReason.Trim());

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = new
                    {
                        Requested = requested,
                        Parked = parked,
                        // already parked, or not part of this contribution
                        Skipped = requested - parked
                    }
                };
            }
            catch (Exception ex)
            {
                return Fail($"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}", 500);
            }
        }

        // ── Park every unparked discrepancy of a type (or all types) ──────
        public async Task<CustomApiResponse> ParkDiscrepanciesAsync(long masterId, string reportType, string parkReason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(parkReason))
                    return Fail("Please provide a reason for parking.", 400);

                var type = (reportType ?? "ALL").Trim().ToUpper();
                var allowed = new[] { "NEWMEMBERS", "WRONGBRANCH", "WRONGCIRCLE", "ALL" };
                if (!allowed.Contains(type))
                    return Fail($"Unknown report type: {reportType}. Use NEWMEMBERS, WRONGBRANCH, WRONGCIRCLE or ALL.", 400);

                var notOpen = await EnsureOpenForParkingAsync(masterId);
                if (notOpen != null) return notOpen;

                var discrepancies = await _repo.GetUnparkedDiscrepancyIdsAsync(masterId);
                var ids = type == "ALL"
                    ? discrepancies.Values.SelectMany(v => v).Distinct().ToList()
                    : discrepancies[type];

                int parked = ids.Count == 0
                    ? 0
                    : await _repo.ParkDetailsAsync(masterId, ids, parkReason.Trim());

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = new { ReportType = type, Parked = parked }
                };
            }
            catch (Exception ex)
            {
                return Fail($"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}", 500);
            }
        }

        // Parking changes what will be forwarded, so it is only allowed while
        // the contribution is still being prepared (status "Uploaded").
        private async Task<CustomApiResponse?> EnsureOpenForParkingAsync(long masterId)
        {
            var status = await _repo.GetMasterStatusAsync(masterId);
            if (status == null)
                return Fail("Contribution master not found", 404);
            if (!status.Trim().Equals("Uploaded", StringComparison.OrdinalIgnoreCase))
                return Fail($"Items can only be parked while the contribution is in 'Uploaded' status (current: {status.Trim()}).", 400);
            return null;
        }

        private static CustomApiResponse Fail(string error, int statusCode) =>
            new CustomApiResponse { IsSucess = false, Error = error, StatusCode = statusCode };

        // Unpark = the discrepancy has been fixed, the row goes back into the batch.
        // It is NOT posted here: the final approval posts the whole contribution.
        public async Task<CustomApiResponse> UnParkItemAsync(long detailId, int currentUserId)
        {
            try
            {
                var detail = await _repo.GetByIdAsync(detailId);
                if (detail == null)
                    return Fail("Contribution detail not found", 404);

                if (!detail.isParked)
                    return Fail("Item is not parked", 400);

                var status = (await _repo.GetMasterStatusAsync(detail.ContributionMasterId))?.Trim();
                if (status == null)
                    return Fail("Contribution master not found", 404);

                if (!status.Equals("Uploaded", StringComparison.OrdinalIgnoreCase) &&
                    !status.Equals("Forwarded", StringComparison.OrdinalIgnoreCase))
                    return Fail($"Items can only be unparked while the contribution is 'Uploaded' or 'Forwarded' (current: {status}).", 400);

                // The discrepancy must really be fixed before the row goes back.
                if (!int.TryParse(detail.StaffNo?.Trim(), out int staffNo) ||
                    !int.TryParse(detail.DpCode?.Trim(), out int dpCode))
                    return Fail("Invalid data in contribution detail record", 400);

                if (await _repo.GetMemberIdByStaffNoAsync(staffNo) == null)
                    return Fail($"Member not found for StaffNo {staffNo}", 400);

                if (await _repo.GetBranchIdByDpCodeAsync(dpCode) == null)
                    return Fail($"Branch not found for DpCode {dpCode}", 400);

                if (await _repo.GetCircleIdByCircleCodeAsync(detail.Circle) == null)
                    return Fail($"Circle not found for circle code {detail.Circle}", 400);

                detail.isParked = false;
                detail.UnParkedon = DateTime.Now;

                _repo.Update(detail);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = "Item unparked. It will be posted when the contribution is approved."
                };
            }
            catch (Exception ex)
            {
                return Fail($"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}", 500);
            }
        }
    }
}
