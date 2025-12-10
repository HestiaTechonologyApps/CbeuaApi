using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
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
            List<RefundContributionDTO> refundContributionDTOs = new List<RefundContributionDTO>();
            var refundContributions = await _repo.GetAllAsync();

            foreach (var refundContribution in refundContributions)
            {
                RefundContributionDTO refundContributionDTO = await ConvertRefundContributionToDTO(refundContribution);
                refundContributionDTOs.Add(refundContributionDTO);


            }

            return refundContributionDTOs;
        }

        public async Task<RefundContributionDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var refunContributionDTO = await ConvertRefundContributionToDTO(q);
            return refunContributionDTO;
        }

        public async Task<RefundContributionDTO> CreateAsync(RefundContribution refundContribution)
        {
            await _repo.AddAsync(refundContribution);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<RefundContribution>(
               tableName: AuditTableName,
               action: "create",
               recordId: refundContribution.RefundContributionId,
               oldEntity: null,
               newEntity: refundContribution,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertRefundContributionToDTO(refundContribution);
        }

        private async Task<RefundContributionDTO> ConvertRefundContributionToDTO(RefundContribution refundContribution)
        {
            RefundContributionDTO refundContributionDTO = new RefundContributionDTO();
            refundContributionDTO.RefundContributionId = refundContribution.RefundContributionId;
           // refundContributionDTO.StaffNo = refundContribution.StaffNo;
            refundContributionDTO.StateId = refundContribution.StateId;
            refundContributionDTO.DesignationId = refundContribution.DesignationId;
          //  refundContributionDTO.DeathDate = refundContribution.DeathDate;
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
            return refundContributionDTO;
        }

        public async Task<bool> UpdateAsync(RefundContribution refundContribution)
        {
            var oldentity = await _repo.GetByIdAsync(refundContribution.RefundContributionId);
            _repo.Detach(oldentity);
            _repo.Update(refundContribution);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<RefundContribution>(
               tableName: AuditTableName,
               action: "update",
               recordId: refundContribution.RefundContributionId,
               oldEntity: oldentity,
               newEntity: refundContribution,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var refund = await _repo.GetByIdAsync(id);
            if (refund == null) return false;
            _repo.Delete(refund);
            await _auditRepository.LogAuditAsync<RefundContribution>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: refund.RefundContributionId,
               oldEntity: refund,
               newEntity: refund,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
