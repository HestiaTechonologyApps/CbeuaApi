using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using System;
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

        public async Task<CustomApiResponse> UnParkItemAsync(long detailId, int currentUserId)
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

                if (!detail.isParked)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Item is not parked",
                        StatusCode = 400
                    };

                // Parse values from detail
                if (!int.TryParse(detail.StaffNo, out int staffNo) ||
                    !int.TryParse(detail.Year, out int year) ||
                    !int.TryParse(detail.Month, out int month) ||
                    !int.TryParse(detail.DpCode, out int dpCode))
                {
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Invalid data in contribution detail record",
                        StatusCode = 400
                    };
                }

                // Get MemberId from StaffNo
                int? memberId = await _repo.GetMemberIdByStaffNoAsync(staffNo);
                if (memberId == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = $"Member not found for StaffNo {staffNo}",
                        StatusCode = 400
                    };

                // Check if already posted to Accounts
                bool alreadyPosted = await _repo.AccountsEntryExistsAsync(memberId.Value, year, month);
                if (alreadyPosted)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution already exists in accounts for this member",
                        StatusCode = 400
                    };

                // Get BranchId from DpCode
                int? branchId = await _repo.GetBranchIdByDpCodeAsync(dpCode);
                if (branchId == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = $"Branch not found for DpCode {dpCode}",
                        StatusCode = 400
                    };

                // Get CircleId from CircleCode
                int? circleId = await _repo.GetCircleIdByCircleCodeAsync(detail.Circle);
                if (circleId == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = $"Circle not found for circle code {detail.Circle}",
                        StatusCode = 400
                    };

                // Post to Accounts
                var account = new Accounts
                {
                    CircleId = circleId.Value,
                    BranchId = branchId.Value,
                    MemeberId = memberId.Value,
                    MonthCode = month,
                    YearOf = year,
                    Amount = detail.Amount,
                    Reference = "Bank File",
                    Remark = $"{detail.ContributionMasterId}/{detail.ContributionDetailId}",
                    TransMode = 8
                };

                await _repo.AddAccountAsync(account);

                // Unpark the detail
                detail.isParked = false;
                detail.UnParkedon = DateTime.Now;

                _repo.Update(detail);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = "Item unparked and posted to accounts successfully"
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}