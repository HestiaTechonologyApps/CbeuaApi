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
    public class DeathClaimService : IDeathClaimService
    {
        private readonly IDeathClaimRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "DEATH_CLAIM";

        public DeathClaimService(IDeathClaimRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<DeathClaimDTO>> GetAllAsync()
        {
            return await _repo.GetQueryableDeathClaims().ToListAsync();
        }

        public async Task<DeathClaimDTO?> GetByIdAsync(int id)
        {
            var q = _repo.GetQueryableDeathClaims();
            var deathClaim = await q.Where(dc => dc.DeathClaimId == id).FirstOrDefaultAsync();
            return deathClaim;
        }

        public async Task<DeathClaimDTO> CreateAsync(DeathClaim deathClaim)
        {
            deathClaim.IsDeleted = false; // ✅ ENSURE NOT DELETED
            await _repo.AddAsync(deathClaim);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<DeathClaim>(
               tableName: AuditTableName,
               action: "create",
               recordId: deathClaim.DeathClaimId,
               oldEntity: null,
               newEntity: deathClaim,
               changedBy: "System" // Replace with actual user info
            );

            // Return the full DTO with joined data
            return await GetByIdAsync(deathClaim.DeathClaimId)
                   ?? new DeathClaimDTO();
        }

        public async Task<bool> UpdateAsync(DeathClaim deathClaim)
        {
            var oldentity = await _repo.GetByIdAsync(deathClaim.DeathClaimId);
            if (oldentity == null || oldentity.IsDeleted) return false; // ✅ CHECK IF DELETED

            _repo.Detach(oldentity);
            _repo.Update(deathClaim);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<DeathClaim>(
               tableName: AuditTableName,
               action: "update",
               recordId: deathClaim.DeathClaimId,
               oldEntity: oldentity,
               newEntity: deathClaim,
               changedBy: "System" // Replace with actual user info
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deathClaim = await _repo.GetByIdAsync(id);
            if (deathClaim == null || deathClaim.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            var oldEntity = CloneDeathClaim(deathClaim); // ✅ CLONE FOR AUDIT

            // ✅ SOFT DELETE
            deathClaim.IsDeleted = true;
            _repo.Update(deathClaim);

            await _auditRepository.LogAuditAsync<DeathClaim>(
               tableName: AuditTableName,
               action: "delete",
               recordId: deathClaim.DeathClaimId,
               oldEntity: oldEntity,
               newEntity: deathClaim,
               changedBy: "System" // Replace with actual user info
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private DeathClaim CloneDeathClaim(DeathClaim deathClaim)
        {
            return new DeathClaim
            {
                DeathClaimId = deathClaim.DeathClaimId,
                MemberId = deathClaim.MemberId,
                StateId = deathClaim.StateId,
                DesignationId = deathClaim.DesignationId,
                DeathDate = deathClaim.DeathDate,
                Nominee = deathClaim.Nominee,
                NomineeRelation = deathClaim.NomineeRelation,
                NomineeIDentity = deathClaim.NomineeIDentity,
                DDNO = deathClaim.DDNO,
                DDDATE = deathClaim.DDDATE,
                Amount = deathClaim.Amount,
                LastContribution = deathClaim.LastContribution,
                YearOF = deathClaim.YearOF,
                IsDeleted = deathClaim.IsDeleted
            };
        }

        public async Task<PagedResult<DeathClaimDTO>> GetPagedDeathClaimsAsync(DeathClaimPaginationParams parameters)
        {
            var query = _repo.GetQueryableDeathClaims();

            if (parameters.DeathClaimId.HasValue && parameters.DeathClaimId.Value > 0)
                query = query.Where(dc => dc.DeathClaimId == parameters.DeathClaimId.Value);

            if (parameters.MemberId.HasValue && parameters.MemberId.Value > 0)
                query = query.Where(dc => dc.MemberId == parameters.MemberId.Value);

            if (parameters.StateId.HasValue && parameters.StateId.Value > 0)
                query = query.Where(dc => dc.StateId == parameters.StateId.Value);

            if (parameters.DesignationId.HasValue && parameters.DesignationId.Value > 0)
                query = query.Where(dc => dc.DesignationId == parameters.DesignationId.Value);

            if (parameters.YearOF.HasValue && parameters.YearOF.Value > 0)
                query = query.Where(dc => dc.YearOF == parameters.YearOF.Value);

            var allClaims = await query.ToListAsync();

            IEnumerable<DeathClaimDTO> filteredClaims = allClaims;

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();

                filteredClaims = allClaims.Where(dc =>
                    dc.DeathClaimId.ToString().Contains(searchLower) ||
                    dc.StaffNo.ToString().Contains(searchLower) ||
                    (!string.IsNullOrEmpty(dc.MemberName) && dc.MemberName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(dc.DesignationName) && dc.DesignationName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(dc.StateName) && dc.StateName.ToLower().Contains(searchLower)) ||
                    dc.Amount.ToString().Contains(searchLower) ||
                    dc.YearName.ToString().Contains(searchLower)
                );
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                var sortBy = parameters.SortBy.ToLower();

                filteredClaims = sortBy switch
                {
                    "deathclaimid" => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.DeathClaimId)
                        : filteredClaims.OrderBy(dc => dc.DeathClaimId),
                    "membername" => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.MemberName)
                        : filteredClaims.OrderBy(dc => dc.MemberName),
                    "designationname" => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.DesignationName)
                        : filteredClaims.OrderBy(dc => dc.DesignationName),
                    "statename" => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.StateName)
                        : filteredClaims.OrderBy(dc => dc.StateName),
                    "deathdate" => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.DeathDate)
                        : filteredClaims.OrderBy(dc => dc.DeathDate),
                    "amount" => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.Amount)
                        : filteredClaims.OrderBy(dc => dc.Amount),
                    "yearname" => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.YearName)
                        : filteredClaims.OrderBy(dc => dc.YearName),
                    _ => parameters.SortDescending
                        ? filteredClaims.OrderByDescending(dc => dc.DeathClaimId)
                        : filteredClaims.OrderBy(dc => dc.DeathClaimId)
                };
            }
            else
            {
                filteredClaims = filteredClaims.OrderByDescending(dc => dc.DeathClaimId);
            }

            var totalRecords = filteredClaims.Count();

            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;

            List<DeathClaimDTO> pagedData;

            if (parameters.GetAll)
            {
                pagedData = filteredClaims.ToList();
            }
            else
            {
                pagedData = filteredClaims
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            return new PagedResult<DeathClaimDTO>
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

                var oldEntity = CloneDeathClaim(entry); 

                entry.isApproved = approve;
                entry.ApprovedBy = currentUserId.ToString();
                entry.ApprovedDate = DateTime.Now;

                _repo.Update(entry);

                await _auditRepository.LogAuditAsync<DeathClaim>(
                    tableName: AuditTableName,
                    action: "update",
                    recordId: entry.DeathClaimId,
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
                        ? new { Message = "death claim approved successfully" }
                        : new { Message = "death claim rejected successfully" }
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