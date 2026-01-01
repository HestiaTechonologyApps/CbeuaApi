using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class DayQuoteService : IDayQuoteService
    {
        private readonly IDayQuoteRepository _repo;
        private readonly IAuditRepository _auditRepository;

        public String AuditTableName { get; set; } = "DAYQUOTE";
        public DayQuoteService(IDayQuoteRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            this._auditRepository = auditRepository;
        }

        public async Task<List<DayQuoteDTO>> GetAllAsync()
        {
              List<DayQuoteDTO> dayQuoteDTOs = new List<DayQuoteDTO>();

            var dayquotes = await _repo.GetAllAsync();

            foreach (var dayquote in dayquotes)
            {
                DayQuoteDTO dayQuoteDTO = await ConvertDayQuoteToDTO(dayquote);
                dayQuoteDTOs.Add(dayQuoteDTO);


            }

            return dayQuoteDTOs;
        }

        public async Task<DayQuoteDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var dayQuoteDTO = await ConvertDayQuoteToDTO(q);
            return dayQuoteDTO;

        }

        public async Task<DayQuoteDTO> CreateAsync(DayQuote dayQuote)
        {
            await _repo.AddAsync(dayQuote);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<DayQuote>(
               tableName: AuditTableName,
               action: "create",
               recordId: dayQuote.DayQuoteId,
               oldEntity: null,
               newEntity: dayQuote,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertDayQuoteToDTO(dayQuote);
        }

        private async Task<DayQuoteDTO> ConvertDayQuoteToDTO(DayQuote dayQuote)
        {
            DayQuoteDTO dayQuoteDTO = new DayQuoteDTO();
            dayQuoteDTO.DayQuoteId = dayQuote.DayQuoteId;
            dayQuoteDTO.ToDayQuote = dayQuote.ToDayQuote;
            dayQuoteDTO.Day= dayQuote.Day;
            dayQuoteDTO.MonthCode = dayQuote.MonthCode;
            dayQuoteDTO.UnformatedContent = dayQuote.UnformatedContent;
            return dayQuoteDTO;
        }

        public async Task<bool> UpdateAsync(DayQuote dayQuote)
        {
            var oldentity = await _repo.GetByIdAsync(dayQuote.DayQuoteId);
            _repo.Detach(oldentity);
            _repo.Update(dayQuote);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<DayQuote>(
               tableName: AuditTableName,
               action: "update",
               recordId: dayQuote.DayQuoteId,
               oldEntity: oldentity,
               newEntity: dayQuote,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dayQuote = await _repo.GetByIdAsync(id);
            if (dayQuote == null) return false;
            _repo.Delete(dayQuote);
            await _auditRepository.LogAuditAsync<DayQuote>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: dayQuote.DayQuoteId,
               oldEntity: dayQuote,
               newEntity: dayQuote,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }

       
    }
}
