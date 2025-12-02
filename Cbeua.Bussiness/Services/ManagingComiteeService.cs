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
    public class ManagingComiteeService : IManagingComiteeService
    {
        private readonly IManagingComiteeRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public String AuditTableName { get; set; } = "MANAGINGCOMITEE";
        public ManagingComiteeService(IManagingComiteeRepository repo, IAuditRepository auditRepository, ICurrentUserService currentUser)
        {
            _repo = repo;
            this._auditRepository = auditRepository;
            _currentUser = currentUser;
        }

        public async Task<ManagingComiteeDTO> CreateAsync(ManagingComitee managingComitee)
        {
            managingComitee.CompanyId = int.Parse(_currentUser.CompanyId);
            await _repo.AddAsync(managingComitee);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<ManagingComitee>(
                tableName: AuditTableName,
                action: "create",
                recordId: managingComitee.ManagingComiteeId,
                oldEntity: null,
                newEntity: managingComitee,
                changedBy: _currentUser.Email.ToString() // Replace with actual user info
            );
            return await ConvertManagingComiteeToDTO(managingComitee);
        }

        private async Task<ManagingComiteeDTO> ConvertManagingComiteeToDTO(ManagingComitee managingComitee)
        {
            ManagingComiteeDTO managingComiteeDTO = new ManagingComiteeDTO();
            managingComiteeDTO.ManagingComiteeId = managingComitee.ManagingComiteeId;
            managingComiteeDTO.ManagingComitteeName = managingComitee.ManagingComitteeName;
            managingComiteeDTO.Position = managingComitee.Position;
            managingComiteeDTO.Description1 = managingComitee.Description1;
            managingComiteeDTO.Description2 = managingComitee.Description2;
            managingComiteeDTO.imageLocation = managingComitee.imageLocation;
            managingComiteeDTO.CompanyId = managingComitee.CompanyId;
            managingComiteeDTO.order = managingComitee.order;
            return managingComiteeDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var managingComitee = await _repo.GetByIdAsync(id);

            if (managingComitee == null) return false;
            _repo.Delete(managingComitee);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<ManagingComitee>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: managingComitee.ManagingComiteeId,
               oldEntity: managingComitee,
               newEntity: managingComitee,
               changedBy: _currentUser.Email.ToString() // Replace with actual user info
           );
            return true;
        }

        public async Task<List<ManagingComiteeDTO>> GetAllAsync()
        {
            var managingcomitee = await _repo.GetAllAsync();
            return managingcomitee.ToList();
        }

        public async Task<ManagingComiteeDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdDTOAsync(id);
            return q;
        }

        public async Task<bool> UpdateAsync(ManagingComitee managingComitee)
        {
            var oldentity = await _repo.GetByIdAsync(managingComitee.ManagingComiteeId);
            _repo.Detach(oldentity);
            _repo.Update(managingComitee);
            managingComitee.CompanyId = int.Parse(_currentUser.CompanyId);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<ManagingComitee>(
              tableName: AuditTableName,
              action: "update",
              recordId: managingComitee.ManagingComiteeId,
              oldEntity: oldentity,
              newEntity: managingComitee,
              changedBy: _currentUser.Email.ToString() // Replace with actual user info
          );
            return true;
        }
    }
}
