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
    public class MonthService : IMonthService
    {
        private readonly IMonthRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "MONTH";

        public MonthService(IMonthRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<MonthDTO>> GetAllAsync()
        {
            List<MonthDTO> monthDTOs = new List<MonthDTO>();
            // ✅ GET ONLY NON-DELETED MONTHS
            var months = await _repo.GetAllActiveAsync();
            foreach (var month in months)
            {
                MonthDTO monthDTO = await ConvertMonthToDTO(month);
                monthDTOs.Add(monthDTO);
            }
            return monthDTOs;
        }

        public async Task<MonthDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null || q.IsDeleted) return null; // ✅ CHECK IF DELETED
            var monthDTO = await ConvertMonthToDTO(q);
            return monthDTO;
        }

        public async Task<MonthDTO> CreateAsync(Month month)
        {
            // ✅ TRIM INPUT
            month.MonthName = month.MonthName?.Trim() ?? "";
            month.Abbrivation = month.Abbrivation?.Trim() ?? "";

            // ✅ VALIDATE: Check for duplicate name
            if (await _repo.ExistsByNameAsync(month.MonthName))
            {
                throw new InvalidOperationException($"A month with the name '{month.MonthName}' already exists.");
            }

            // ✅ VALIDATE: Check for duplicate abbreviation
            if (await _repo.ExistsByAbbreviationAsync(month.Abbrivation))
            {
                throw new InvalidOperationException($"A month with the abbreviation '{month.Abbrivation}' already exists.");
            }

            month.IsDeleted = false; // ✅ ENSURE NOT DELETED
            await _repo.AddAsync(month);
            await _repo.SaveChangesAsync();

            await this._auditRepository.LogAuditAsync<Month>(
               tableName: AuditTableName,
               action: "create",
               recordId: month.MonthCode,
               oldEntity: null,
               newEntity: month,
               changedBy: "System"
           );
            return await ConvertMonthToDTO(month);
        }

        private async Task<MonthDTO> ConvertMonthToDTO(Month month)
        {
            MonthDTO monthDTO = new MonthDTO();
            monthDTO.Abbrivation = month.Abbrivation;
            monthDTO.MonthCode = month.MonthCode;
            monthDTO.MonthName = month.MonthName;
            monthDTO.IsDeleted = month.IsDeleted; // ✅ ADDED
            return monthDTO;
        }

        // ✅ ADDED CLONE METHOD FOR AUDIT
        private Month CloneMonth(Month month)
        {
            return new Month
            {
                MonthCode = month.MonthCode,
                MonthName = month.MonthName,
                Abbrivation = month.Abbrivation,
                IsDeleted = month.IsDeleted
            };
        }

        public async Task<bool> UpdateAsync(Month month)
        {
            var oldEntity = await _repo.GetByIdAsync(month.MonthCode);
            if (oldEntity == null || oldEntity.IsDeleted) return false; // ✅ CHECK IF DELETED

            // ✅ TRIM INPUT
            month.MonthName = month.MonthName?.Trim() ?? "";
            month.Abbrivation = month.Abbrivation?.Trim() ?? "";

            // ✅ VALIDATE: Check for duplicate name (excluding current month)
            if (await _repo.ExistsByNameAsync(month.MonthName, month.MonthCode))
            {
                throw new InvalidOperationException($"A month with the name '{month.MonthName}' already exists.");
            }

            // ✅ VALIDATE: Check for duplicate abbreviation (excluding current month)
            if (await _repo.ExistsByAbbreviationAsync(month.Abbrivation, month.MonthCode))
            {
                throw new InvalidOperationException($"A month with the abbreviation '{month.Abbrivation}' already exists.");
            }

            // ✅ CLONE FOR AUDIT
            var oldMonthClone = CloneMonth(oldEntity);

            // Update fields
            oldEntity.MonthName = month.MonthName;
            oldEntity.Abbrivation = month.Abbrivation;

            _repo.Update(oldEntity);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<Month>(
               tableName: AuditTableName,
               action: "update",
               recordId: oldEntity.MonthCode,
               oldEntity: oldMonthClone,
               newEntity: oldEntity,
               changedBy: "System"
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var month = await _repo.GetByIdAsync(id);
            if (month == null || month.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            // ✅ CLONE FOR AUDIT
            var oldMonth = CloneMonth(month);

            // ✅ SOFT DELETE
            month.IsDeleted = true;
            _repo.Update(month);

            await _auditRepository.LogAuditAsync<Month>(
               tableName: AuditTableName,
               action: "delete",
               recordId: month.MonthCode,
               oldEntity: oldMonth,
               newEntity: month,
               changedBy: "System"
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}