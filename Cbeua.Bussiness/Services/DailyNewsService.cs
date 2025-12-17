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
    public class DailyNewsService : IDailyNewsService
    {
        private readonly IDailyNewsRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUserService;
        public String AuditTableName { get; set; } = "DAILY_NEWS";
        public DailyNewsService(IDailyNewsRepository repo, IAuditRepository auditRepository, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<DailyNewsDTO>> GetAllAsync()
        {
            List<DailyNewsDTO> dailyNewsDTOs = new List<DailyNewsDTO>();

            var dailyNews = await _repo.GetAllAsync();

            foreach (var dailyNew in dailyNews)
            {
                DailyNewsDTO dailyNewsDTO = await ConvertDailyNewsToDTO(dailyNew);
                dailyNewsDTOs.Add(dailyNewsDTO);


            }

            return dailyNewsDTOs;
        }

        public async Task<DailyNewsDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var dailyNewsDTO = await ConvertDailyNewsToDTO(q);
            return dailyNewsDTO;
        }

        public async Task<DailyNewsDTO> CreateAsync(DailyNews dailyNews)
        {
            dailyNews.CompanyId = int.Parse(_currentUserService.CompanyId);
            await _repo.AddAsync(dailyNews);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<DailyNews>(
                tableName: AuditTableName,
                action: "create",
                recordId: dailyNews.DailyNewsId,
                oldEntity: null,
                newEntity: dailyNews,
                changedBy: _currentUserService.Email.ToString() // Replace with actual user info
            );
            return await ConvertDailyNewsToDTO(dailyNews);
        }

        private async Task<DailyNewsDTO> ConvertDailyNewsToDTO(DailyNews dailyNews)
        {
            DailyNewsDTO dailyNewsDTO = new DailyNewsDTO();
            dailyNewsDTO.DailyNewsId = dailyNews.DailyNewsId;
            dailyNewsDTO.Title = dailyNews.Title;
            dailyNewsDTO.Description = dailyNews.Description;
            dailyNewsDTO.NewsDate = dailyNews.NewsDate;
            dailyNewsDTO.CompanyId = dailyNews.CompanyId;
            dailyNewsDTO.IsActive = dailyNews.IsActive;
            dailyNewsDTO.IsDeleted = dailyNews.IsDeleted;
            dailyNewsDTO.CreatedOn = dailyNews.CreatedOn;
            dailyNewsDTO.CreatedBy = dailyNews.CreatedBy;
            return dailyNewsDTO;
        }

        public async Task<bool> UpdateAsync(DailyNews dailyNews)
        {
            var oldentity = await _repo.GetByIdAsync(dailyNews.DailyNewsId);
            _repo.Detach(oldentity);
            _repo.Update(dailyNews);
            dailyNews.CompanyId = int.Parse(_currentUserService.CompanyId);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<DailyNews>(
               tableName: AuditTableName,
               action: "update",
               recordId: dailyNews.DailyNewsId,
               oldEntity: oldentity,
               newEntity: dailyNews,
               changedBy: _currentUserService.Email.ToString() // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dailyNews = await _repo.GetByIdAsync(id);
            if (dailyNews == null) return false;
            _repo.Delete(dailyNews);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<DailyNews>(
                tableName: AuditTableName,
                action: "Delete",
                recordId: dailyNews.DailyNewsId,
                oldEntity: dailyNews,
                newEntity: dailyNews,
                changedBy: _currentUserService.Email.ToString() // Replace with actual user info
            );
            return true;
        }
    }
}
