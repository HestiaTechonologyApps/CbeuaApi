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
    public class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "SUPPORTTICKET";
        public SupportTicketService(ISupportTicketRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<SupportTicketDTO>> GetAllAsync()
        {
            List<SupportTicketDTO> supportTicketDTOs = new List<SupportTicketDTO>();
            var supportTickets = await _repo.GetAllAsync();

            foreach (var supportTicket in supportTickets)
            {
                SupportTicketDTO supportTicketDTO = await ConvertSupportTicketToDTO(supportTicket);
                supportTicketDTOs.Add(supportTicketDTO);


            }

            return supportTicketDTOs;
        }

        public async Task<SupportTicketDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var supportTicketDTO = await ConvertSupportTicketToDTO(q);
            return supportTicketDTO;
        }

        public async Task<SupportTicketDTO> CreateAsync(SupportTicket supportTicket)
        {
            await _repo.AddAsync(supportTicket);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<SupportTicket>(
               tableName: AuditTableName,
               action: "create",
               recordId: supportTicket.SupportTicketId,
               oldEntity: null,
               newEntity: supportTicket,
               changedBy: "System" // Replace with actual user info

               );
            return await ConvertSupportTicketToDTO(supportTicket);
        }

        private async Task<SupportTicketDTO> ConvertSupportTicketToDTO(SupportTicket supportTicket)
        {
            SupportTicketDTO supportTicketDTO = new SupportTicketDTO();
            supportTicketDTO.SupportTicketId = supportTicket.SupportTicketId;
            supportTicketDTO.SupportTicketNum = supportTicket.SupportTicketNum;
            supportTicketDTO.Description = supportTicket.Description;
            supportTicketDTO.Priority = supportTicket.Priority;
            supportTicketDTO.Duration = supportTicket.Duration;
            supportTicketDTO.DeveloperRemark = supportTicket.DeveloperRemark;
            supportTicketDTO.isApproved = supportTicket.isApproved;
            supportTicketDTO.ApprovedByUserId = supportTicket.ApprovedByUserId;
            supportTicketDTO.ApprovedDate = supportTicket.ApprovedDate;
            return supportTicketDTO;
        }

        public async Task<bool> UpdateAsync(SupportTicket supportTicket)
        {
            var oldentity = await _repo.GetByIdAsync(supportTicket.SupportTicketId);
            _repo.Detach(oldentity);
            _repo.Update(supportTicket);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<SupportTicket>(
               tableName: AuditTableName,
            action: "update",
               recordId: supportTicket.SupportTicketId,
            oldEntity: oldentity,
               newEntity: supportTicket,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supportTicket = await _repo.GetByIdAsync(id);
            if (supportTicket == null) return false;
            _repo.Delete(supportTicket);
            await _auditRepository.LogAuditAsync<SupportTicket>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: supportTicket.SupportTicketId,
               oldEntity: supportTicket,
               newEntity: supportTicket,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
