using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Rewrite;
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
        public String AuditTableName { get; set; } = "DEATHCLAIM";
        public DeathClaimService(IDeathClaimRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            this._auditRepository = auditRepository;
        }

        public async Task<List<DeathClaimDTO>> GetAllAsync()
        {
            return _repo.GetQueryableDeathClaims().ToList();
        }

        public async Task<DeathClaimDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var deathClaimDTO = await ConvertDeathClaimToDTO(q);
            return deathClaimDTO;
        }

        public async Task<DeathClaimDTO> CreateAsync(DeathClaim deathClaim)
        {
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
            return await ConvertDeathClaimToDTO(deathClaim);
        }

        private async Task<DeathClaimDTO> ConvertDeathClaimToDTO(DeathClaim deathClaim)
        {
            DeathClaimDTO deathClaimDTO = new DeathClaimDTO();
            deathClaimDTO.DeathClaimId = deathClaim.DeathClaimId;
            deathClaimDTO.MemberId = deathClaim.MemberId;
            deathClaimDTO.StateId = deathClaim.StateId;
            deathClaimDTO.DesignationId = deathClaim.DesignationId;
            deathClaimDTO.DeathDate = deathClaim.DeathDate;
            deathClaimDTO.Nominee = deathClaim.Nominee;
            deathClaimDTO.NomineeRelation = deathClaim.NomineeRelation;
            deathClaimDTO.NomineeIDentity = deathClaim.NomineeIDentity;
            deathClaimDTO.DDNO = deathClaim.DDNO;
            deathClaimDTO.DDDATE = deathClaim.DDDATE;
            deathClaimDTO.Amount = deathClaim.Amount;
            deathClaimDTO.LastContribution = deathClaim.LastContribution;
            deathClaimDTO.YearOF = deathClaim.YearOF;
            return deathClaimDTO;
        }

        public async Task<bool> UpdateAsync(DeathClaim deathClaim)
        {
            var oldentity = await _repo.GetByIdAsync(deathClaim.DeathClaimId);
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
            if (deathClaim == null) return false;
            _repo.Delete(deathClaim);
            await _auditRepository.LogAuditAsync<DeathClaim>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: deathClaim.DeathClaimId,
               oldEntity: deathClaim,
               newEntity: deathClaim,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
