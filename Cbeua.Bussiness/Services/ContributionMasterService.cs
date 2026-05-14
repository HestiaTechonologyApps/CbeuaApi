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
    public class ContributionMasterService : IContributionMasterService
    {
        private readonly IContributionMasterRepository _repo;

        public ContributionMasterService(IContributionMasterRepository repo)
        {
            _repo = repo;
        }

        public async Task<CustomApiResponse> GetAllAsync()
        {
            try
            {
                var masters = await _repo.GetAllAsync();
                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = masters
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

        public async Task<CustomApiResponse> GetByIdAsync(long masterId)
        {
            try
            {
                var master = await _repo.GetByIdAsync(masterId);
                if (master == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution master not found",
                        StatusCode = 404
                    };

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = master
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

      

        public async Task<CustomApiResponse> DeleteAsync(long masterId)
        {
            try
            {
                var master = await _repo.GetById(masterId);
                if (master == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution master not found",
                        StatusCode = 404
                    };

                if (master.ContributionStatus?.Trim().ToUpper() == "A")
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Approved contributions cannot be deleted",
                        StatusCode = 400
                    };

                _repo.Delete(master);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = "Contribution deleted successfully"
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

        public async Task<CustomApiResponse> ForwardAsync(long masterId)
        {
            try
            {
                var master = await _repo.GetById(masterId);
                if (master == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution master not found",
                        StatusCode = 404
                    };

                if (master.ContributionStatus?.Trim().ToUpper() != "UPLOADED")
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Only Uploaded contributions can be forwarded for approval",
                        StatusCode = 400
                    };

                master.ContributionStatus = "FORWARD";

                _repo.Update(master);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = "Contribution forwarded for approval successfully"
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

        public async Task<CustomApiResponse> ApproveAsync(long masterId, int currentUserId, bool approve)
        {
            try
            {
                var master = await _repo.GetById(masterId);
                if (master == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution master not found",
                        StatusCode = 404
                    };

                if (master.ContributionStatus?.Trim().ToUpper() != "FORWARD")
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Only forwarded contributions can be approved or rejected",
                        StatusCode = 400
                    };

                int approvedCount = 0;

                if (approve)
                {
                    var details = await _repo.GetDetailsWithLookupsAsync(masterId);
                    approvedCount = details.Count;

                    if (!details.Any())
                        return new CustomApiResponse
                        {
                            IsSucess = false,
                            Error = "No details found to approve.",
                            StatusCode = 400
                        };

                    var accounts = details.Select(d => new Accounts
                    {
                        CircleId = d.CircleId,
                        BranchId = d.BranchId,
                        MemeberId = d.MemberId,
                        MonthCode = d.MonthCode,
                        YearOf = d.YearOf,
                        Amount = d.Amount,
                        Reference = "Bank File",
                        Remark = "Monthly Contribution",
                        TransMode = 8,
                    }).ToList();

                    await _repo.AddAccountsRangeAsync(accounts);

                    master.isApproved = true;
                    master.ContributionStatus = "A";
                    master.ApprovedDate = DateTime.UtcNow.ToString();
                    master.ApprovedBy = currentUserId.ToString();
                }
                else
                {
                    master.isApproved = false;
                    master.ContributionStatus = "R";
                    master.ApprovedDate = DateTime.UtcNow.ToString();
                    master.ApprovedBy = currentUserId.ToString();
                }

                _repo.Update(master);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = approve
                        ? new { Message = "Contribution approved and posted to accounts successfully", ApprovedCount = approvedCount }
                        : new { Message = "Contribution rejected successfully" }
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
        public async Task<CustomApiResponse> GetParkedDetailsAsync(long masterId, int pageNumber, int pageSize)
        {
            try
            {
                var master = await _repo.GetByIdAsync(masterId);
                if (master == null)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution master not found",
                        StatusCode = 404
                    };

                var parked = await _repo.GetParkedDetailsAsync(masterId);

                var totalCount = parked.Count;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var paged = parked
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = new
                    {
                        MasterId = masterId,
                        TotalCount = totalCount,
                        TotalPages = totalPages,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        Items = paged
                    }
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