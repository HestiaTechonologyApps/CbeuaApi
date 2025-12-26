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
            publicPageDTO.PublicPageId = publicPageDTO.PublicPageId;

            /* ========== NAVBAR ========== */
            publicPageDTO.NavBrandTitle = publicPageDTO.NavBrandTitle;
            publicPageDTO.NavBrandSubTitle = publicPageDTO.NavBrandSubTitle;
            publicPageDTO.NavLogoUrl = publicPageDTO.NavLogoUrl;
            publicPageDTO.NavLogoAlt = publicPageDTO.NavLogoAlt;
            publicPageDTO.NavMenuHead = publicPageDTO.NavMenuHead;

            publicPageDTO.NavHomeLabel = publicPageDTO.NavHomeLabel;
            publicPageDTO.NavAboutLabel = publicPageDTO.NavAboutLabel;
            publicPageDTO.NavRulesLabel = publicPageDTO.NavRulesLabel;
            publicPageDTO.NavDownloadsLabel = publicPageDTO.NavDownloadsLabel;
            publicPageDTO.NavCommitteeLabel = publicPageDTO.NavCommitteeLabel;
            publicPageDTO.NavClaimsLabel = publicPageDTO.NavClaimsLabel;
            publicPageDTO.NavContactLabel = publicPageDTO.NavContactLabel;

            publicPageDTO.NavLoginLabel = publicPageDTO.NavLoginLabel;
            publicPageDTO.NavLoginIcon = publicPageDTO.NavLoginIcon;

            publicPageDTO.NavPhoneIcon = publicPageDTO.NavPhoneIcon;
            publicPageDTO.NavPhoneValue = publicPageDTO.NavPhoneValue;
            publicPageDTO.NavEmailIcon = publicPageDTO.NavEmailIcon;
            publicPageDTO.NavEmailValue = publicPageDTO.NavEmailValue;

            /* ========== HOME – HERO ========== */
            publicPageDTO.HomeHeroBadge = publicPageDTO.HomeHeroBadge;
            publicPageDTO.HomeHeroTitle = publicPageDTO.HomeHeroTitle;
            publicPageDTO.HomeHeroLine1 = publicPageDTO.HomeHeroLine1;
            publicPageDTO.HomeHeroHighlight = publicPageDTO.HomeHeroHighlight;
            publicPageDTO.HomeHeroLine3 = publicPageDTO.HomeHeroLine3;
            publicPageDTO.HomeHeroDescription = publicPageDTO.HomeHeroDescription;

            publicPageDTO.HomePrimaryBtnLabel = publicPageDTO.HomePrimaryBtnLabel;
            publicPageDTO.HomePrimaryBtnRoute = publicPageDTO.HomePrimaryBtnRoute;
            publicPageDTO.HomeSecondaryBtnLabel = publicPageDTO.HomeSecondaryBtnLabel;
            publicPageDTO.HomeSecondaryBtnRoute = publicPageDTO.HomeSecondaryBtnRoute;

            publicPageDTO.HomeHeroImageUrl = publicPageDTO.HomeHeroImageUrl;
            publicPageDTO.HomeHeroImageAlt = publicPageDTO.HomeHeroImageAlt;

            /* ========== HOME – FEATURES ========== */
            publicPageDTO.HomeFeatureHeading = publicPageDTO.HomeFeatureHeading;
            publicPageDTO.HomeFeatureLabel = publicPageDTO.HomeFeatureLabel;
            publicPageDTO.HomeFeatureTitle = publicPageDTO.HomeFeatureTitle;
            publicPageDTO.HomeFeatureSubTitle = publicPageDTO.HomeFeatureSubTitle;
            publicPageDTO.HomeFeatureItemsJson = publicPageDTO.HomeFeatureItemsJson;

            /* ========== HOME – ABOUT ========== */
            publicPageDTO.HomeAboutLabel = publicPageDTO.HomeAboutLabel;
            publicPageDTO.HomeAboutTitle = publicPageDTO.HomeAboutTitle;
            publicPageDTO.HomeAboutParagraph = publicPageDTO.HomeAboutParagraph;

            /* ========== FOOTER ========== */
            publicPageDTO.FooterBrandShortName = publicPageDTO.FooterBrandShortName;
            publicPageDTO.FooterBrandSubTitle = publicPageDTO.FooterBrandSubTitle;
            publicPageDTO.FooterBrandDescription = publicPageDTO.FooterBrandDescription;
            publicPageDTO.FooterLogoAlt = publicPageDTO.FooterLogoAlt;

            publicPageDTO.FooterAddressLine1 = publicPageDTO.FooterAddressLine1;
            publicPageDTO.FooterAddressLine2 = publicPageDTO.FooterAddressLine2;

            publicPageDTO.FooterPhoneIcon = publicPageDTO.FooterPhoneIcon;
            publicPageDTO.FooterPhoneValue = publicPageDTO.FooterPhoneValue;

            publicPageDTO.FooterEmailIcon = publicPageDTO.FooterEmailIcon;
            publicPageDTO.FooterEmailValue = publicPageDTO.FooterEmailValue;

            publicPageDTO.FooterQuickLinksJson = publicPageDTO.FooterQuickLinksJson;
            publicPageDTO.FooterOfficeHoursJson = publicPageDTO.FooterOfficeHoursJson;
            publicPageDTO.FooterCopyrightText = publicPageDTO.FooterCopyrightText;

            /* ========== PRIVACY POLICY ========== */
            publicPageDTO.PrivacyHeroBadge = publicPageDTO.PrivacyHeroBadge;
            publicPageDTO.PrivacyHeroTitle = publicPageDTO.PrivacyHeroTitle;
            publicPageDTO.PrivacyHeroSubTitle = publicPageDTO.PrivacyHeroSubTitle;

            publicPageDTO.PrivacyHeading1 = publicPageDTO.PrivacyHeading1;
            publicPageDTO.PrivacyPara1 = publicPageDTO.PrivacyPara1;
            publicPageDTO.PrivacyPara2 = publicPageDTO.PrivacyPara2;

            publicPageDTO.PrivacyHeading2 = publicPageDTO.PrivacyHeading2;
            publicPageDTO.PrivacyPara3 = publicPageDTO.PrivacyPara3;

            publicPageDTO.PrivacyHeading3 = publicPageDTO.PrivacyHeading3;
            publicPageDTO.PrivacyLine1 = publicPageDTO.PrivacyLine1;
            publicPageDTO.PrivacyLine2 = publicPageDTO.PrivacyLine2;
            publicPageDTO.PrivacyLine3 = publicPageDTO.PrivacyLine3;
            publicPageDTO.PrivacyLine4 = publicPageDTO.PrivacyLine4;
            publicPageDTO.PrivacyLine5 = publicPageDTO.PrivacyLine5;
            publicPageDTO.PrivacyLine6 = publicPageDTO.PrivacyLine6;



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
