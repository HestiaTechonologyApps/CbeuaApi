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
            var months = await _repo.GetAllAsync();

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
            if (q == null) return null;
            var monthDTO = await ConvertMonthToDTO(q);
            return monthDTO;
        }

        public async Task<MonthDTO> CreateAsync(Month month)
        {
            await _repo.AddAsync(month);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Month>(
               tableName: AuditTableName,
            action: "create",
               recordId: month.MonthCode,
            oldEntity: null,
               newEntity: month,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertMonthToDTO(month);
        }

        private async Task<MonthDTO> ConvertMonthToDTO(Month month)
        {
            MonthDTO monthDTO = new MonthDTO();
            monthDTO.Abbrivation = month.Abbrivation;
            monthDTO.MonthCode = month.MonthCode;
            monthDTO.MonthName = month.MonthName;
            return monthDTO;
        }

        public async Task<bool> UpdateAsync(Month month)
        {
            var oldentity = await _repo.GetByIdAsync(month.MonthCode);
            _repo.Detach(oldentity);
            _repo.Update(month);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Month>(
               tableName: AuditTableName,
               action: "update",
               recordId: month.MonthCode,
               oldEntity: oldentity,
               newEntity: month,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var month = await _repo.GetByIdAsync(id);
            if (month == null) return false;
            _repo.Delete(month);
            await _auditRepository.LogAuditAsync<Month>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: month.MonthCode,
               oldEntity: month,
               newEntity: month,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
