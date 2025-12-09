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
    public class UserRoleRightService : IUserRoleRightService
    {
        private readonly IUserRoleRightRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "USERROLERIGHT";
        public UserRoleRightService(IUserRoleRightRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<UserRoleRightDTO> CreateAsync(UserRoleRight userRoleRight)
        {
            await _repo.AddAsync(userRoleRight);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<UserRoleRight>(
               tableName: AuditTableName,
               action: "create",
               recordId: userRoleRight.UserRoleRightId,
               oldEntity: null,
               newEntity: userRoleRight,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertUserRoleRightToDTO(userRoleRight);
        }

        private async Task<UserRoleRightDTO> ConvertUserRoleRightToDTO(UserRoleRight userRoleRight)
        {
            UserRoleRightDTO userRoleRightDTO = new UserRoleRightDTO();
            userRoleRightDTO.UserRoleRightId = userRoleRight.UserRoleRightId;
            userRoleRightDTO.ControllerName = userRoleRight.ControllerName;
            userRoleRightDTO.ActionName = userRoleRight.ActionName;
            userRoleRightDTO.UserTypeID = userRoleRight.UserTypeID;
            return userRoleRightDTO;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userRoleRight = await _repo.GetByIdAsync(id);
            if (userRoleRight == null) return false;
            _repo.Delete(userRoleRight);
            await _auditRepository.LogAuditAsync<UserRoleRight>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: userRoleRight.UserRoleRightId,
               oldEntity: userRoleRight,
               newEntity: userRoleRight,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserRoleRightDTO>> GetAllAsync()
        {

            List<UserRoleRightDTO> userRoleRightDTOs = new List<UserRoleRightDTO>();

            var userRoleRights = await _repo.GetAllAsync();

            foreach (var userRoleRight in userRoleRights)
            {
                UserRoleRightDTO userRoleRightDTO = await ConvertUserRoleRightToDTO(userRoleRight);
                userRoleRightDTOs.Add(userRoleRightDTO);


            }

            return userRoleRightDTOs;
        }

        public async Task<UserRoleRightDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var userRoleRightDTO = await ConvertUserRoleRightToDTO(q);
            return userRoleRightDTO;
        }

        public async Task<bool> UpdateAsync(UserRoleRight userRoleRight)
        {
            var oldentity = await _repo.GetByIdAsync(userRoleRight.UserRoleRightId);
            _repo.Detach(oldentity);
            _repo.Update(userRoleRight);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<UserRoleRight>(
               tableName: AuditTableName,
               action: "update",
               recordId: userRoleRight.UserRoleRightId,
               oldEntity: oldentity,
               newEntity: userRoleRight,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }
    }
}
