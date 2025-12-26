using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class PublicPageService : IPublicPageService
    {
        private readonly IPublicPageRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public const string AuditTableName = "PublicPages";
        public PublicPageService(IPublicPageRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<PublicPageDTO> CreateAsync(PublicPage publicPage)
        {
            await _repo.AddAsync(publicPage);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<PublicPage>(
               tableName: AuditTableName,
            action: "create",
               recordId: publicPage.PublicPageId,
            oldEntity: null,
               newEntity: publicPage,
               changedBy: "System" // Replace with actual user info

           );
            return await ConvertPublicToDTO(publicPage);
        }

        private async Task<PublicPageDTO> ConvertPublicToDTO(PublicPage publicPage)
        {
            PublicPageDTO publicPageDTO = new PublicPageDTO();
            publicPageDTO.PublicPageId = publicPage.PublicPageId;
            publicPageDTO.NavbarTitle = publicPage.NavbarTitle;
            publicPageDTO.NavbarSubTitle = publicPage.NavbarSubTitle;
            publicPageDTO.NavbarLogoAlt = publicPage.NavbarLogoAlt;
            publicPageDTO.NavbarMenuHead = publicPage.NavbarMenuHead;
            publicPageDTO.NavbarMenuJson = publicPage.NavbarMenuJson;
            publicPageDTO.NavbarLoginLabel = publicPage.NavbarLoginLabel;
            publicPageDTO.NavbarLoginIconClass = publicPage.NavbarLoginIconClass;
            publicPageDTO.NavbarPhone = publicPage.NavbarPhone;
            publicPageDTO.NavbarEmail = publicPage.NavbarEmail;
            publicPageDTO.HeroBadge = publicPage.HeroBadge;
            publicPageDTO.HeroTitleLine1 = publicPage.HeroTitleLine1;
            publicPageDTO.HeroTitleHighlight = publicPage.HeroTitleHighlight;
            publicPageDTO.HeroTitleLine3 = publicPage.HeroTitleLine3;
            publicPageDTO.HeroDescription = publicPage.HeroDescription;
            publicPageDTO.HeroPrimaryBtnLabel = publicPage.HeroPrimaryBtnLabel;
            publicPageDTO.HeroPrimaryBtnRoute = publicPage.HeroPrimaryBtnRoute;
            publicPageDTO.HeroSecondaryBtnLabel = publicPage.HeroSecondaryBtnLabel;
            publicPageDTO.HeroSecondaryBtnRoute = publicPage.HeroSecondaryBtnRoute;
            publicPageDTO.FooterShortName = publicPage.FooterShortName;
            publicPageDTO.FooterSubtitle = publicPage.FooterSubtitle;
            publicPageDTO.FooterDescription = publicPage.FooterDescription;
            publicPageDTO.FooterAddress1 = publicPage.FooterAddress1;
            publicPageDTO.FooterAddress2 = publicPage.FooterAddress2;
            publicPageDTO.FooterPhone = publicPage.FooterPhone;
            publicPageDTO.FooterEmail = publicPage.FooterEmail;
            publicPageDTO.FooterQuickLinksJson = publicPage.FooterQuickLinksJson;
            publicPageDTO.FooterOfficeHoursJson = publicPage.FooterOfficeHoursJson;
            publicPageDTO.FooterBottomBarText = publicPage.FooterBottomBarText;
            
            return publicPageDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var publicpage = await _repo.GetByIdAsync(id);
            if (publicpage == null) return false;
            _repo.Delete(publicpage);
            await _auditRepository.LogAuditAsync<PublicPage>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: publicpage.PublicPageId,
               oldEntity: publicpage,
               newEntity: publicpage,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<PublicPageDTO>> GetAllAsync()
        {
            var publicPages = _repo.GetQueryablePublicPageList();
            return await publicPages.ToListAsync();
        }

        public Task<PublicPageDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(PublicPage publicPage)
        {
            var oldentity = await _repo.GetByIdAsync(publicPage.PublicPageId);
            _repo.Detach(oldentity);
            _repo.Update(publicPage);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<PublicPage>(
               tableName: AuditTableName,
               action: "update",
               recordId: publicPage.PublicPageId,
               oldEntity: oldentity,
               newEntity: publicPage,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }
    }
}
