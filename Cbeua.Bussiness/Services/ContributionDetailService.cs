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

        // >>> NEW CODE START
        // We also need the monthly-contribution repository, because it has the
        // methods that recount the totals and the new member count on the master record.
        private readonly IMonthlyContributionRepository _monthlyRepo;
        // <<< NEW CODE END

        // >>> OLD CODE (commented out, replaced by the constructor below which also receives _monthlyRepo)
        //public ContributionDetailService(IContributionDetailRepository repo)
        //{
        //    _repo = repo;
        //}
        // <<< END OLD CODE

        // >>> NEW CODE START
        public ContributionDetailService(IContributionDetailRepository repo, IMonthlyContributionRepository monthlyRepo)
        {
            _repo = repo;
            _monthlyRepo = monthlyRepo;
        }

        // Recounts the header numbers of a contribution after a row was parked or unparked.
        // Call it AFTER the row change has been saved to the database.
        //   updateNewMemberCount = true  -> also refresh the "New members" number (used by Unpark)
        private async Task RefreshMasterNumbersAsync(long masterId, bool updateNewMemberCount)
        {
            var master = await _monthlyRepo.GetContributionMasterByIdAsync(masterId);
            if (master == null) return;

            if (updateNewMemberCount)
            {
                // Unpark only works for people who are now members, so the live "new members"
                // count goes down. Clear the caches first so the count uses fresh data.
                _monthlyRepo.InvalidateMemberLookup();
                _monthlyRepo.InvalidateReportCache(masterId);

                var liveNewMembers = await _monthlyRepo.GetNewMemberCountAsync(masterId);
                master.NewMemberCount = liveNewMembers.ToString();
                await _monthlyRepo.UpdateContributionMasterAsync(master);
            }

            // Total amount and total entries = only the rows that are NOT parked.
            await _monthlyRepo.RecalculateMasterTotalsAsync(masterId);

            // One save writes the master changes.
            await _monthlyRepo.SaveChangesAsync();
        }
        // <<< NEW CODE END

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

                // >>> NEW CODE START
                // The row is now parked, so the total amount and total entries must go down.
                // If this refresh fails, the row is still parked, so we do not return an error.
                string parkNote = "Item parked successfully";
                try
                {
                    await RefreshMasterNumbersAsync(detail.ContributionMasterId, updateNewMemberCount: false);
                }
                catch (Exception)
                {
                    parkNote = "Item parked successfully (header totals could not be refreshed)";
                }
                // <<< NEW CODE END

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    // >>> CHANGED: was  Value = "Item parked successfully"
                    Value = parkNote
                    // <<< END CHANGED
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

                // >>> NEW CODE START
                // The row is now unparked and posted to accounts, so:
                //   total amount  +this row's amount
                //   total entries +1
                //   new members   recounted (this person is a member now, so the count goes down)
                // If this refresh fails, the unpark itself already succeeded, so we do not return an error
                // (otherwise the user would click Unpark again and get "Item is not parked").
                string unparkNote = "Item unparked and posted to accounts successfully";
                try
                {
                    await RefreshMasterNumbersAsync(detail.ContributionMasterId, updateNewMemberCount: true);
                }
                catch (Exception)
                {
                    unparkNote = "Item unparked and posted to accounts successfully (header totals could not be refreshed)";
                }
                // <<< NEW CODE END

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    // >>> CHANGED: was  Value = "Item unparked and posted to accounts successfully"
                    Value = unparkNote
                    // <<< END CHANGED
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