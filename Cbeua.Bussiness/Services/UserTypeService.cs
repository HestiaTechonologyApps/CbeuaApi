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
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "USERTYPE";
        public UserTypeService(IUserTypeRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<UserTypeDTO>> GetAllAsync()
        {
            List<UserTypeDTO> userTypeDTOs = new List<UserTypeDTO>();
            var userTypes = await _repo.GetAllAsync();

            foreach (var userType in userTypes)
            {
                UserTypeDTO userTypeDTO = await ConvertUserTypeToDTO(userType);
                userTypeDTOs.Add(userTypeDTO);


            }

            return userTypeDTOs;


        }

        public async Task<UserTypeDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var userTypeDTO = await ConvertUserTypeToDTO(q);
            return userTypeDTO;
        }

        public async Task<UserTypeDTO> CreateAsync(UserType userType)
        {
            await _repo.AddAsync(userType);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<UserType>(
               tableName: AuditTableName,
               action: "create",
               recordId: userType.UserTypeId,
               oldEntity: null,
               newEntity: userType,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertUserTypeToDTO(userType);
        }

        private async Task<UserTypeDTO> ConvertUserTypeToDTO(UserType userType)
        {
            UserTypeDTO userTypeDTO = new UserTypeDTO();
            userTypeDTO.UserTypeId = userType.UserTypeId;
            userTypeDTO.Abbreviation = userType.Abbreviation;
            userTypeDTO.Description = userType.Description;
            return userTypeDTO;
        }

        public async Task<bool> UpdateAsync(UserType userType)
        {
            var oldentity = await _repo.GetByIdAsync(userType.UserTypeId);
            _repo.Detach(oldentity);
            _repo.Update(userType);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<UserType>(
               tableName: AuditTableName,
               action: "update",
               recordId: userType.UserTypeId,
               oldEntity: oldentity,
               newEntity: userType,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userType = await _repo.GetByIdAsync(id);
            if (userType == null) return false;
            _repo.Delete(userType);
            await _auditRepository.LogAuditAsync<UserType>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: userType.UserTypeId,
               oldEntity: userType,
               newEntity: userType,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
