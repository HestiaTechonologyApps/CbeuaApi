using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class MainPageService : IMainPageService
    {
        private readonly IMainPageRepository _repo;
        private readonly IAuditRepository _auditRepository;
        

        public String AuditTableName { get; set; } = "MAINPAGE";
        public MainPageService(IMainPageRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            this._auditRepository = auditRepository;
            
        }

        public async Task<List<MainPageDTO>> GetAllAsync()
        {
            var mainPages = _repo.GetQueryableMainPageList();

            return await mainPages.ToListAsync();
        }


        public async Task<MainPageDTO?> GetByIdAsync(int id)
        {
            var dto = await _repo
                .GetQueryableMainPageList()
                .Where(x => x.MainPageId == id)
                .FirstOrDefaultAsync();

            if (dto == null)
                return null;

            dto.AuditLogs = await _auditRepository
                .GetAuditLogsForEntityAsync("MAINPAGE", dto.MainPageId);

            return dto;
        }


        public async Task<MainPageDTO> CreateAsync(MainPage mainPage)
        {
           // mainPage.CompanyId = int.Parse(_currentUser.CompanyId);
            await _repo.AddAsync(mainPage);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<MainPage>(
                tableName: AuditTableName,
                action: "create",
                recordId: mainPage.MainPageId,
                oldEntity: null,
                newEntity: mainPage,
                 changedBy: "System" // Replace with actual user info
            );
            return await ConvertMainPageToDTO(mainPage);
        }

        private async Task<MainPageDTO> ConvertMainPageToDTO(MainPage mainPage)
        {
            MainPageDTO mainPageDTO = new MainPageDTO();
            mainPageDTO.MainPageId = mainPage.MainPageId;
            mainPageDTO.CorouselImage1 = mainPage.CorouselImage1;
            mainPageDTO.CorouselImage2 = mainPage.CorouselImage2;
            mainPageDTO.CorouselImage3 = mainPage.CorouselImage3;
            mainPageDTO.MainText = mainPage.MainText;
            mainPageDTO.Slogan = mainPage.Slogan;
            mainPageDTO.LogoImage1 = mainPage.LogoImage1;
            mainPageDTO.LogoImage2 = mainPage.LogoImage2;
            mainPageDTO.ContactDesc1 = mainPage.ContactDesc1;
            mainPageDTO.ContactDesc2 = mainPage.ContactDesc2;
            mainPageDTO.ContactLine1 = mainPage.ContactLine1;
            mainPageDTO.ContactLine2 = mainPage.ContactLine2;
            mainPageDTO.ContactLine3 = mainPage.ContactLine3;
            mainPageDTO.Phonenum = mainPage.Phonenum;
            mainPageDTO.Faxnum = mainPage.Faxnum;
            mainPageDTO.Website = mainPage.Website;
            mainPageDTO.Email = mainPage.Email;
            mainPageDTO.RulesRegulation = mainPage.RulesRegulation;
            mainPageDTO.DayQuote = mainPage.DayQuote;
            mainPageDTO.CompanyId = mainPage.CompanyId;
            return mainPageDTO;
        }

        public async Task<bool> UpdateAsync(MainPage mainPage)
        {
            var oldentity = await _repo.GetByIdAsync(mainPage.MainPageId);
            _repo.Detach(oldentity);
            _repo.Update(mainPage);
          //  mainPage.CompanyId = int.Parse(_currentUser.CompanyId);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<MainPage>(
              tableName: AuditTableName,
              action: "update",
              recordId: mainPage.MainPageId,
              oldEntity: oldentity,
              newEntity: mainPage,
               changedBy: "System" // Replace with actual user info
          );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var mainPage = await _repo.GetByIdAsync(id);

            if (mainPage == null) return false;
            _repo.Delete(mainPage);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<MainPage>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: mainPage.MainPageId,
               oldEntity: mainPage,
               newEntity: mainPage,
                changedBy: "System" // Replace with actual user info
           );
            return true;
        }
    }
}
