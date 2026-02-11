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
            //  GET ONLY NON-DELETED YEARS
            var yearMasters = await _repo.GetAllActiveAsync();
            foreach (var yearMaster in yearMasters)
            {
                YearMasterDTO yearMasterDTO = await ConvertYearMasterToDTO(yearMaster);
                yearMasterDTOs.Add(yearMasterDTO);
            }
            return yearMasterDTOs;
        }

        public async Task<YearMasterDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null || q.IsDeleted) return null; // ✅ CHECK IF DELETED
            var yearMasterDTO = await ConvertYearMasterToDTO(q);
            return yearMasterDTO;
        }

        public async Task<YearMasterDTO> CreateAsync(YearMaster yearMaster)
        {
            //  VALIDATE: Check for duplicate year name
            if (await _repo.ExistsByYearNameAsync(yearMaster.YearName))
            {
                throw new InvalidOperationException($"Year {yearMaster.YearName} already exists.");
            }

            //  VALIDATE: Year should be reasonable (e.g., between 1900 and 2100)
            if (yearMaster.YearName < 1900 || yearMaster.YearName > 2100)
            {
                throw new InvalidOperationException($"Year must be between 1900 and 2100.");
            }

            yearMaster.IsDeleted = false; //  ENSURE NOT DELETED
            await _repo.AddAsync(yearMaster);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<YearMaster>(
               tableName: AuditTableName,
               action: "create",
               recordId: yearMaster.YearOf,
               oldEntity: null,
               newEntity: yearMaster,
               changedBy: "System"
           );
            return await ConvertYearMasterToDTO(yearMaster);
        }

        private async Task<YearMasterDTO> ConvertYearMasterToDTO(YearMaster yearMaster)
        {
            YearMasterDTO yearMasterDTO = new YearMasterDTO();
            yearMasterDTO.YearOf = yearMaster.YearOf;
            yearMasterDTO.YearName = yearMaster.YearName;
            yearMasterDTO.IsDeleted = yearMaster.IsDeleted; // ✅ ADDED
            return yearMasterDTO;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private YearMaster CloneYearMaster(YearMaster yearMaster)
        {
            return new YearMaster
            {
                YearOf = yearMaster.YearOf,
                YearName = yearMaster.YearName,
                IsDeleted = yearMaster.IsDeleted
            };
        }

        public async Task<bool> UpdateAsync(YearMaster yearMaster)
        {
            var oldEntity = await _repo.GetByIdAsync(yearMaster.YearOf);
            if (oldEntity == null || oldEntity.IsDeleted) return false; //  CHECK IF DELETED

            //  VALIDATE: Check for duplicate year name (excluding current year)
            if (await _repo.ExistsByYearNameAsync(yearMaster.YearName, yearMaster.YearOf))
            {
                throw new InvalidOperationException($"Year {yearMaster.YearName} already exists.");
            }

            //  VALIDATE: Year should be reasonable
            if (yearMaster.YearName < 1900 || yearMaster.YearName > 2100)
            {
                throw new InvalidOperationException($"Year must be between 1900 and 2100.");
            }

            //  CLONE FOR AUDIT
            var oldYearClone = CloneYearMaster(oldEntity);

            // Update fields
            oldEntity.YearName = yearMaster.YearName;

            _repo.Update(oldEntity);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<YearMaster>(
               tableName: AuditTableName,
               action: "update",
               recordId: oldEntity.YearOf,
               oldEntity: oldYearClone,
               newEntity: oldEntity,
               changedBy: "System"
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var yearMaster = await _repo.GetByIdAsync(id);
            if (yearMaster == null || yearMaster.IsDeleted) return false; 

            // CLONE FOR AUDIT
            var oldYear = CloneYearMaster(yearMaster);

            // SOFT DELETE
            yearMaster.IsDeleted = true;
            _repo.Update(yearMaster);

            await _auditRepository.LogAuditAsync<YearMaster>(
               tableName: AuditTableName,
               action: "delete",
               recordId: yearMaster.YearOf,
               oldEntity: oldYear,
               newEntity: yearMaster,
               changedBy: "System"
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}