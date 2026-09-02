using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class RefundContributionService : IRefundContributionService
    {
        private readonly IRefundContributionRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "REFUNDCONTRIBUTION";

        public RefundContributionService(IRefundContributionRepository repository, IAuditRepository auditRepository)
        {
            _repo = repository;
            _auditRepository = auditRepository;
        }

        public async Task<List<RefundContributionDTO>> GetAllAsync()
        {
            return _repo.QueryableRefundContributions().ToList();
        }

        public async Task<RefundContributionDTO?> GetByIdAsync(int id)
        {
            var q = _repo.QueryableRefundContributionById(id);
            return await q.AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<List<RefundContributionDTO>> GetByMemberIdAsync(int memberId)
        {
            var q = _repo.QueryableRefundContributionByMemberId(memberId);
            return await q.AsNoTracking().ToListAsync();
        }

        public async Task<RefundContributionDTO> CreateAsync(RefundContribution refundContribution)
        {
            var eligibility = await _repo.GetMemberRefundEligibilityAsync(refundContribution.MemberId);
            if (refundContribution.Amount > eligibility.AvailableAmount)
                throw new InvalidOperationException(
                    $"Refund amount ({refundContribution.Amount}) exceeds the available balance ({eligibility.AvailableAmount}) for this member.");

            refundContribution.IsDeleted = false; 
            await _repo.AddAsync(refundContribution);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<RefundContribution>(
               tableName: AuditTableName,
               action: "create",
               recordId: refundContribution.RefundContributionId,
               oldEntity: null,
               newEntity: refundContribution,
               changedBy: "System"
            );

            return await ConvertRefundContributionToDTO(refundContribution);
        }

        private async Task<RefundContributionDTO> ConvertRefundContributionToDTO(RefundContribution refundContribution)
        {
            RefundContributionDTO refundContributionDTO = new RefundContributionDTO();
            refundContributionDTO.RefundContributionId = refundContribution.RefundContributionId;
            refundContributionDTO.MemberId = refundContribution.MemberId;
            refundContributionDTO.StateId = refundContribution.StateId;
            refundContributionDTO.DesignationId = refundContribution.DesignationId;
            refundContributionDTO.RefundNO = refundContribution.RefundNO;
            refundContributionDTO.BranchNameOFTime = refundContribution.BranchNameOFTime;
            refundContributionDTO.DPCODEOfTime = refundContribution.DPCODEOfTime;
            refundContributionDTO.Type = refundContribution.Type;
            refundContributionDTO.Remark = refundContribution.Remark;
            refundContributionDTO.DDNO = refundContribution.DDNO;
            refundContributionDTO.DDDATE = refundContribution.DDDATE;
            refundContributionDTO.Amount = refundContribution.Amount;
            refundContributionDTO.LastContribution = refundContribution.LastContribution;
            refundContributionDTO.YearOF = refundContribution.YearOF;
            refundContributionDTO.IsDeleted = refundContribution.IsDeleted; // ✅ ADDED
            // Note: MemberName, StaffNo, StateName, and DesignationName won't be populated here
            // They will only be populated when using QueryableRefundContributions()
            return refundContributionDTO;
        }

        public async Task<bool> UpdateAsync(RefundContribution refundContribution)
        {
            var oldentity = await _repo.GetByIdAsync(refundContribution.RefundContributionId);
            if (oldentity == null || oldentity.IsDeleted) return false; 

            var eligibility = await _repo.GetMemberRefundEligibilityAsync(
                refundContribution.MemberId,
                excludeRefundContributionId: refundContribution.RefundContributionId);
            if (refundContribution.Amount > eligibility.AvailableAmount)
                throw new InvalidOperationException(
                    $"Refund amount ({refundContribution.Amount}) exceeds the available balance ({eligibility.AvailableAmount}) for this member.");

            _repo.Detach(oldentity);
            _repo.Update(refundContribution);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<RefundContribution>(
               tableName: AuditTableName,
               action: "update",
               recordId: refundContribution.RefundContributionId,
               oldEntity: oldentity,
               newEntity: refundContribution,
               changedBy: "System" 
            );

            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var refund = await _repo.GetByIdAsync(id);
            if (refund == null || refund.IsDeleted) return false; 
            var oldEntity = CloneRefundContribution(refund); 

            
            refund.IsDeleted = true;
            _repo.Update(refund);

            await _auditRepository.LogAuditAsync<RefundContribution>(
               tableName: AuditTableName,
               action: "delete",
               recordId: refund.RefundContributionId,
               oldEntity: oldEntity,
               newEntity: refund,
               changedBy: "System" // Replace with actual user info
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        private RefundContribution CloneRefundContribution(RefundContribution refund)
        {
            return new RefundContribution
            {
                RefundContributionId = refund.RefundContributionId,
                StateId = refund.StateId,
                MemberId = refund.MemberId,
                DesignationId = refund.DesignationId,
                RefundNO = refund.RefundNO,
                BranchNameOFTime = refund.BranchNameOFTime,
                DPCODEOfTime = refund.DPCODEOfTime,
                Type = refund.Type,
                Remark = refund.Remark,
                DDNO = refund.DDNO,
                DDDATE = refund.DDDATE,
                Amount = refund.Amount,
                LastContribution = refund.LastContribution,
                YearOF = refund.YearOF,
                IsDeleted = refund.IsDeleted
            };
        }

        public async Task<PagedResult<RefundContributionDTO>> GetPagedRefundContributionsAsync(RefundContributionPaginationParams parameters)
        {
            var query = _repo.QueryableRefundContributions();

            if (parameters.RefundContributionId.HasValue && parameters.RefundContributionId.Value > 0)
                query = query.Where(rc => rc.RefundContributionId == parameters.RefundContributionId.Value);

            if (parameters.MemberId.HasValue && parameters.MemberId.Value > 0)
                query = query.Where(rc => rc.MemberId == parameters.MemberId.Value);

            if (parameters.StateId.HasValue && parameters.StateId.Value > 0)
                query = query.Where(rc => rc.StateId == parameters.StateId.Value);

            if (parameters.DesignationId.HasValue && parameters.DesignationId.Value > 0)
                query = query.Where(rc => rc.DesignationId == parameters.DesignationId.Value);

            if (parameters.YearOF.HasValue && parameters.YearOF.Value > 0)
                query = query.Where(rc => rc.YearOF == parameters.YearOF.Value);

            var allRefunds = query.ToList();

            IEnumerable<RefundContributionDTO> filteredRefunds = allRefunds;

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();

                filteredRefunds = allRefunds.Where(rc =>
                    rc.RefundContributionId.ToString().Contains(searchLower) ||
                    rc.StaffNo.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(rc.MemberName) && rc.MemberName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(rc.DesignationName) && rc.DesignationName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(rc.StateName) && rc.StateName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(rc.RefundNO) && rc.RefundNO.ToLower().Contains(searchLower)) ||
                    rc.Amount.ToString().Contains(searchLower) ||
                    rc.YearName.ToString().Contains(searchLower)
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                var sortBy = parameters.SortBy.ToLower();

                filteredRefunds = sortBy switch
                {
                    "refundcontributionid" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.RefundContributionId)
                        : filteredRefunds.OrderBy(rc => rc.RefundContributionId),
                    "staffno" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.StaffNo)
                        : filteredRefunds.OrderBy(rc => rc.StaffNo),
                    "membername" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.MemberName)
                        : filteredRefunds.OrderBy(rc => rc.MemberName),
                    "designationname" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.DesignationName)
                        : filteredRefunds.OrderBy(rc => rc.DesignationName),
                    "statename" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.StateName)
                        : filteredRefunds.OrderBy(rc => rc.StateName),
                    "refundno" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.RefundNO)
                        : filteredRefunds.OrderBy(rc => rc.RefundNO),
                    "amount" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.Amount)
                        : filteredRefunds.OrderBy(rc => rc.Amount),
                    "yearname" => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.YearName)
                        : filteredRefunds.OrderBy(rc => rc.YearName),
                    _ => parameters.SortDescending
                        ? filteredRefunds.OrderByDescending(rc => rc.RefundContributionId)
                        : filteredRefunds.OrderBy(rc => rc.RefundContributionId)
                };
            }
            else
            {
                filteredRefunds = filteredRefunds.OrderByDescending(rc => rc.RefundContributionId);
            }

            var totalRecords = filteredRefunds.Count();

            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;

            List<RefundContributionDTO> pagedData;

            if (parameters.GetAll)
            {
                pagedData = filteredRefunds.ToList();
            }
            else
            {
                pagedData = filteredRefunds
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            return new PagedResult<RefundContributionDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = parameters.GetAll ? totalRecords : pageSize
            };

        }
        public async Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve)
        {
            try
            {
                var entry = await _repo.GetByIdAsync(id);
                if (entry == null || entry.IsDeleted)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Refund contribution not found",
                        StatusCode = 404
                    };

                if (entry.isApproved)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Refund contribution is already approved",
                        StatusCode = 400
                    };

                var oldEntity = CloneRefundContribution(entry); 

                entry.isApproved = approve;
                entry.ApprovedBy = currentUserId.ToString();
                entry.ApprovedDate = DateTime.Now;

                _repo.Update(entry);

                await _auditRepository.LogAuditAsync<RefundContribution>(
                    tableName: AuditTableName,
                    action: "update",
                    recordId: entry.RefundContributionId,
                    oldEntity: oldEntity,
                    newEntity: entry,
                    changedBy: currentUserId.ToString()
                );

                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = approve
                        ? new { Message = "Refund contribution approved successfully" }
                        : new { Message = "Refund contribution rejected successfully" }
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

        public async Task<CustomApiResponse> GetMemberEligibilityAsync(int memberId, int? excludeRefundContributionId = null)
        {
            try
            {
                var eligibility = await _repo.GetMemberRefundEligibilityAsync(memberId, excludeRefundContributionId);
                return new CustomApiResponse { IsSucess = true, StatusCode = 200, Value = eligibility };
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