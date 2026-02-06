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
    }
}