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
    public class DirectPaymentService : IDirectPaymentService
    {
        private readonly IDirectPaymentRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "DIRECTPAYMENT";
        public DirectPaymentService(IDirectPaymentRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<DirectPaymentDTO>> GetAllAsync()
        {
            List<DirectPaymentDTO> directPaymentDTOs = new List<DirectPaymentDTO>();
            var directPayments = await _repo.GetAllAsync();

            foreach (var directPayment in directPayments)
            {
                DirectPaymentDTO directPaymentDTO = await ConvertDirectPaymentToDTO(directPayment);
                directPaymentDTOs.Add(directPaymentDTO);


            }

            return directPaymentDTOs;
        }

        public async Task<DirectPaymentDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var directPaymentDTO = await ConvertDirectPaymentToDTO(q);
            return directPaymentDTO;
        }

        public async Task<DirectPaymentDTO> CreateAsync(DirectPayment directPayment)
        {
            await _repo.AddAsync(directPayment);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<DirectPayment>(
               tableName: AuditTableName,
               action: "create",
               recordId: directPayment.DirectPaymentId,
               oldEntity: null,
               newEntity: directPayment,
               changedBy: "System" // Replace with actual user info
               );
            return await ConvertDirectPaymentToDTO(directPayment);
        }

        private async Task<DirectPaymentDTO> ConvertDirectPaymentToDTO(DirectPayment directPayment)
        {
            DirectPaymentDTO directPaymentDTO = new DirectPaymentDTO();
            directPaymentDTO.DirectPaymentId = directPayment.DirectPaymentId;
            directPaymentDTO.MemberId = directPayment.MemberId;
            directPaymentDTO.Amount = directPayment.Amount;
            directPaymentDTO.PaymentDate = directPayment.PaymentDate;
            directPaymentDTO.PaymentMode = directPayment.PaymentMode;
            directPaymentDTO.ReferenceNo = directPayment.ReferenceNo;
            directPaymentDTO.Remarks = directPayment.Remarks;
            directPaymentDTO.CreatedByUserId = directPayment.CreatedByUserId;
            directPaymentDTO.CreatedDate = directPayment.CreatedDate;
            return directPaymentDTO;
        }

        public async Task<bool> UpdateAsync(DirectPayment directPayment)
        {
            var oldentity = await _repo.GetByIdAsync(directPayment.DirectPaymentId);
            _repo.Detach(oldentity);
            _repo.Update(directPayment);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<DirectPayment>(
               tableName: AuditTableName,
            action: "update",
               recordId: directPayment.DirectPaymentId,
            oldEntity: oldentity,
               newEntity: directPayment,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var directPayment = await _repo.GetByIdAsync(id);
            if (directPayment == null) return false;
            _repo.Delete(directPayment);
            await _auditRepository.LogAuditAsync<DirectPayment>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: directPayment.DirectPaymentId,
               oldEntity: directPayment,
               newEntity: directPayment,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
