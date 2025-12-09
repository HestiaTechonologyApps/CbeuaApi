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
    public class YearMasterService : IYearMasterService
    {
        private readonly IYearMasterRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "YEARMASTER";
        public YearMasterService(IYearMasterRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<YearMasterDTO>> GetAllAsync()
        {
            List<YearMasterDTO> yearMasterDTOs = new List<YearMasterDTO>();

            var yearMasters = await _repo.GetAllAsync();

            foreach (var yearMaster in yearMasters)
            {
                YearMasterDTO yearMasterDTO = await ConvertayesrMasterToDTO(yearMaster);
                yearMasterDTOs.Add(yearMasterDTO);


            }

            return yearMasterDTOs;
        }

        public async Task<YearMasterDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var yearMasterDTO = await ConvertayesrMasterToDTO(q);
            return yearMasterDTO;
        }

        public async Task<YearMasterDTO> CreateAsync(YearMaster yearMaster)
        {
            await _repo.AddAsync(yearMaster);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<YearMaster>(
               tableName: AuditTableName,
               action: "create",
               recordId: yearMaster.YearOf,
               oldEntity: null,
               newEntity: yearMaster,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertayesrMasterToDTO(yearMaster);
        }

        private async Task<YearMasterDTO> ConvertayesrMasterToDTO(YearMaster yearMaster)
        {
            YearMasterDTO yearMasterDTO = new YearMasterDTO();
            yearMasterDTO.YearOf = yearMaster.YearOf;
            yearMasterDTO.YearName = yearMaster.YearName;
            return yearMasterDTO;
        }

        public async Task<bool> UpdateAsync(YearMaster yearMaster)
        {
            var oldentity = await _repo.GetByIdAsync(yearMaster.YearOf);
            _repo.Detach(oldentity);
            _repo.Update(yearMaster);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<YearMaster>(
               tableName: AuditTableName,
               action: "update",
               recordId: yearMaster.YearOf,
               oldEntity: oldentity,
               newEntity: yearMaster,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var yearMaster = await _repo.GetByIdAsync(id);
            if (yearMaster == null) return false;
            _repo.Delete(yearMaster);
            await _auditRepository.LogAuditAsync<YearMaster>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: yearMaster.YearOf,
               oldEntity: yearMaster,
               newEntity: yearMaster,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}