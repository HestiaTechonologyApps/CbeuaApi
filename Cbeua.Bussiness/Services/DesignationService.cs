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
    public class DesignationService : IDesignationsService
    {
        private readonly IDesignationRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "DESIGNATION";
        public DesignationService(IDesignationRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<DesignationDTO>> GetAllAsync()
        {
            List<DesignationDTO> designationDTOs = new List<DesignationDTO>();
            var designations = await _repo.GetAllAsync();

            foreach (var designation in designations)
            {
                DesignationDTO designationDTO = await ConvertDesignationToDTO(designation);
                designationDTOs.Add(designationDTO);


            }

            return designationDTOs;
        }

        public async Task<DesignationDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var designationDTO = await ConvertDesignationToDTO(q);
            return designationDTO;
        }

        public async Task<DesignationDTO> CreateAsync(Designation designation)
        {
            await _repo.AddAsync(designation);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Designation>(
               tableName: AuditTableName,
               action: "create",
               recordId: designation.DesignationId,
               oldEntity: null,
               newEntity: designation,
               changedBy: "System" // Replace with actual user info
               );
            return await ConvertDesignationToDTO(designation);
        }

        private async Task<DesignationDTO> ConvertDesignationToDTO(Designation designation)
        {
            DesignationDTO designationDTO = new DesignationDTO();
            designationDTO.DesignationId = designation.DesignationId;

            designationDTO.Name = designation.Name;
            designationDTO.Description = designation.Description;
            return designationDTO;
        }

        public async Task<bool> UpdateAsync(Designation designation)
        {
            var oldentity = await _repo.GetByIdAsync(designation.DesignationId);
            _repo.Detach(oldentity);
            _repo.Update(designation);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Designation>(
               tableName: AuditTableName,
               action: "update",
               recordId: designation.DesignationId,
               oldEntity: oldentity,
               newEntity: designation,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var designation = await _repo.GetByIdAsync(id);
            if (designation == null) return false;
            _repo.Delete(designation);
            await _auditRepository.LogAuditAsync<Designation>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: designation.DesignationId,
               oldEntity: designation,
               newEntity: designation,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
