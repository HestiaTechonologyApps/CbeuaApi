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
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "STATUS";
        public StatusService(IStatusRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<StatusDTO>> GetAllAsync()
        {
            List<StatusDTO> statusDTOs = new List<StatusDTO>();
            var statuses = await _repo.GetAllAsync();

            foreach (var status in statuses)
            {
                StatusDTO statusDTO = await ConvertStatusToDTO(status);
                statusDTOs.Add(statusDTO);


            }

            return statusDTOs;
        }

        public async Task<StatusDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var statusDTO = await ConvertStatusToDTO(q);
            return statusDTO;
        }

        public async Task<StatusDTO> CreateAsync(Status status)
        {
            await _repo.AddAsync(status);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Status>(
               tableName: AuditTableName,
               action: "create",
               recordId: status.StatusId,
               oldEntity: null,
               newEntity: status,
               changedBy: "System" // Replace with actual user info
               );
            return await ConvertStatusToDTO(status);
        }

        private async Task<StatusDTO> ConvertStatusToDTO(Status status)
        {
            StatusDTO statusDTO = new StatusDTO();
            statusDTO.StatusId = status.StatusId;
            statusDTO.Abbreviation = status.Abbreviation;
            statusDTO.Name = status.Name;
            statusDTO.Description = status.Description;
            statusDTO.GroupId = status.GroupId;
            return statusDTO;
        }

        public async Task<bool> UpdateAsync(Status status)
        {
            var oldentity = await _repo.GetByIdAsync(status.StatusId);
            _repo.Detach(oldentity);
            _repo.Update(status);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Status>(
               tableName: AuditTableName,
               action: "update",
               recordId: status.StatusId,
               oldEntity: oldentity,
               newEntity: status,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await _repo.GetByIdAsync(id);
            if (status == null) return false;
            _repo.Delete(status);
            await _auditRepository.LogAuditAsync<Status>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: status.StatusId,
               oldEntity: status,
               newEntity: status,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
