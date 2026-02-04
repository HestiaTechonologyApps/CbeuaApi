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
               changedBy: "System"
           );
            return ConvertPublicToDTO(publicPage);
        }

        private PublicPageDTO ConvertPublicToDTO(PublicPage publicPage)
        {
            PublicPageDTO publicPageDTO = new PublicPageDTO();
            publicPageDTO.PublicPageId = publicPage.PublicPageId;

            /* ========== NAVBAR ========== */
            publicPageDTO.NavBrandTitle = publicPage.NavBrandTitle;
            publicPageDTO.NavBrandSubTitle = publicPage.NavBrandSubTitle;
            publicPageDTO.NavLogoUrl = publicPage.NavLogoUrl;
            publicPageDTO.NavLogoAlt = publicPage.NavLogoAlt;
            publicPageDTO.NavMenuHead = publicPage.NavMenuHead;

            publicPageDTO.NavHomeLabel = publicPage.NavHomeLabel;
            publicPageDTO.NavAboutLabel = publicPage.NavAboutLabel;
            publicPageDTO.NavRulesLabel = publicPage.NavRulesLabel;
            publicPageDTO.NavDownloadsLabel = publicPage.NavDownloadsLabel;
            publicPageDTO.NavCommitteeLabel = publicPage.NavCommitteeLabel;
            publicPageDTO.NavClaimsLabel = publicPage.NavClaimsLabel;
            publicPageDTO.NavContactLabel = publicPage.NavContactLabel;

            publicPageDTO.NavLoginLabel = publicPage.NavLoginLabel;
            publicPageDTO.NavLoginIcon = publicPage.NavLoginIcon;

            publicPageDTO.NavPhoneIcon = publicPage.NavPhoneIcon;
            publicPageDTO.NavPhoneValue = publicPage.NavPhoneValue;
            publicPageDTO.NavEmailIcon = publicPage.NavEmailIcon;
            publicPageDTO.NavEmailValue = publicPage.NavEmailValue;

            /* ========== HOME – HERO ========== */
            publicPageDTO.HomeHeroBadge = publicPage.HomeHeroBadge;
            publicPageDTO.HomeHeroTitle = publicPage.HomeHeroTitle;
            publicPageDTO.HomeHeroLine1 = publicPage.HomeHeroLine1;
            publicPageDTO.HomeHeroHighlight = publicPage.HomeHeroHighlight;
            publicPageDTO.HomeHeroLine3 = publicPage.HomeHeroLine3;
            publicPageDTO.HomeHeroDescription = publicPage.HomeHeroDescription;

            publicPageDTO.HomePrimaryBtnLabel = publicPage.HomePrimaryBtnLabel;
            publicPageDTO.HomePrimaryBtnRoute = publicPage.HomePrimaryBtnRoute;
            publicPageDTO.HomeSecondaryBtnLabel = publicPage.HomeSecondaryBtnLabel;
            publicPageDTO.HomeSecondaryBtnRoute = publicPage.HomeSecondaryBtnRoute;

            publicPageDTO.HomeHeroImageUrl = publicPage.HomeHeroImageUrl;
            publicPageDTO.HomeHeroImageAlt = publicPage.HomeHeroImageAlt;

            /* ========== HOME – FEATURES ========== */
            publicPageDTO.HomeFeatureHeading = publicPage.HomeFeatureHeading;
            publicPageDTO.HomeFeatureLabel = publicPage.HomeFeatureLabel;
            publicPageDTO.HomeFeatureTitle = publicPage.HomeFeatureTitle;
            publicPageDTO.HomeFeatureSubTitle = publicPage.HomeFeatureSubTitle;
            publicPageDTO.HomeFeatureItemsJson = publicPage.HomeFeatureItemsJson;

            /* ========== HOME – ABOUT ========== */
            publicPageDTO.HomeAboutLabel = publicPage.HomeAboutLabel;
            publicPageDTO.HomeAboutTitle = publicPage.HomeAboutTitle;
            publicPageDTO.HomeAboutParagraph = publicPage.HomeAboutParagraph;

            /* ========== NEWS PAGE ========== */
            publicPageDTO.NewsHeroTitle = publicPage.NewsHeroTitle;
            publicPageDTO.NewsHeroSubTitle = publicPage.NewsHeroSubTitle;
            publicPageDTO.NewsBreadcrumbHomeLabel = publicPage.NewsBreadcrumbHomeLabel;
            publicPageDTO.NewsBreadcrumbCurrentLabel = publicPage.NewsBreadcrumbCurrentLabel;
            publicPageDTO.NewsLoadingText = publicPage.NewsLoadingText;
            publicPageDTO.NewsEmptyText = publicPage.NewsEmptyText;
            publicPageDTO.NewsItemsJson = publicPage.NewsItemsJson;
            publicPageDTO.NewsSidebarQuoteTitle = publicPage.NewsSidebarQuoteTitle;
            publicPageDTO.NewsSidebarQuoteText = publicPage.NewsSidebarQuoteText;
            publicPageDTO.NewsQuickLinksJson = publicPage.NewsQuickLinksJson;
            publicPageDTO.NewsSectionHeadingLabel = publicPage.NewsSectionHeadingLabel;
            publicPageDTO.NewsSectionHeadingTitle = publicPage.NewsSectionHeadingTitle;
            publicPageDTO.NewsSectionQuickLinksHead = publicPage.NewsSectionQuickLinksHead;
            publicPageDTO.NewsTag = publicPage.NewsTag;

            /* ========== ABOUT PAGE ========== */
            publicPageDTO.AboutHeaderTitle = publicPage.AboutHeaderTitle;
            publicPageDTO.AboutHeaderSubTitle = publicPage.AboutHeaderSubTitle;
            publicPageDTO.AboutMissionTitle = publicPage.AboutMissionTitle;
            publicPageDTO.AboutMissionIcon = publicPage.AboutMissionIcon;
            publicPageDTO.AboutMissionDescription = publicPage.AboutMissionDescription;
            publicPageDTO.AboutVisionTitle = publicPage.AboutVisionTitle;
            publicPageDTO.AboutVisionIcon = publicPage.AboutVisionIcon;
            publicPageDTO.AboutVisionDescription = publicPage.AboutVisionDescription;
            publicPageDTO.AboutHistoryTitle = publicPage.AboutHistoryTitle;
            publicPageDTO.AboutHistoryIcon = publicPage.AboutHistoryIcon;
            publicPageDTO.AboutHistoryPara1 = publicPage.AboutHistoryPara1;
            publicPageDTO.AboutHistoryPara2 = publicPage.AboutHistoryPara2;
            publicPageDTO.AboutHistoryPara3 = publicPage.AboutHistoryPara3;
            publicPageDTO.AboutHistoryPara4 = publicPage.AboutHistoryPara4;
            publicPageDTO.AboutHistoryPara5 = publicPage.AboutHistoryPara5;
            publicPageDTO.AboutParagraph1 = publicPage.AboutParagraph1;
            publicPageDTO.AboutParagraph2 = publicPage.AboutParagraph2;
            publicPageDTO.AboutParagraph3 = publicPage.AboutParagraph3;
            publicPageDTO.AboutParagraph4 = publicPage.AboutParagraph4;
            publicPageDTO.AboutStatsJson = publicPage.AboutStatsJson;

            /* ========== RULES & REGULATIONS ========== */
            publicPageDTO.RulesHeaderTitle = publicPage.RulesHeaderTitle;
            publicPageDTO.RulesHeaderSubTitle = publicPage.RulesHeaderSubTitle;
            publicPageDTO.RulesPreambleTitle = publicPage.RulesPreambleTitle;
            publicPageDTO.RulesPreamblePara1 = publicPage.RulesPreamblePara1;
            publicPageDTO.RulesPreamblePara2 = publicPage.RulesPreamblePara2;
            publicPageDTO.RulesPreamblePara3 = publicPage.RulesPreamblePara3;
            publicPageDTO.RulesPreamblePara4 = publicPage.RulesPreamblePara4;
            publicPageDTO.RulesPreamblePara5 = publicPage.RulesPreamblePara5;
            publicPageDTO.RulesPreamblePara6 = publicPage.RulesPreamblePara6;
            publicPageDTO.RulesSectionsJson = publicPage.RulesSectionsJson;

            /* ========== DOWNLOADS ========== */
            publicPageDTO.DownloadsHeaderTitle = publicPage.DownloadsHeaderTitle;
            publicPageDTO.DownloadsHeaderSubTitle = publicPage.DownloadsHeaderSubTitle;
            publicPageDTO.DownloadItemsJson = publicPage.DownloadItemsJson;
            publicPageDTO.DownloadsCardTitle = publicPage.DownloadsCardTitle;
            publicPageDTO.DownloadsCardIconClass = publicPage.DownloadsCardIconClass;
            publicPageDTO.DownloadIcon = publicPage.DownloadIcon;
            publicPageDTO.DownloadsContactButtonText = publicPage.DownloadsContactButtonText;

            /* ========== MANAGING COMMITTEE ========== */
            publicPageDTO.CommitteeHeaderTitle = publicPage.CommitteeHeaderTitle;
            publicPageDTO.CommitteeHeaderSubTitle = publicPage.CommitteeHeaderSubTitle;
            publicPageDTO.CommitteeMembersJson = publicPage.CommitteeMembersJson;
            publicPageDTO.ManagingCommitteeHeaderTitle = publicPage.ManagingCommitteeHeaderTitle;
            publicPageDTO.ManagingCommitteeHeaderSubTitle = publicPage.ManagingCommitteeHeaderSubTitle;

            /* ========== CLAIMS PAGE ========== */
            publicPageDTO.ClaimsHeroTitle = publicPage.ClaimsHeroTitle;
            publicPageDTO.ClaimsHeroSubTitle = publicPage.ClaimsHeroSubTitle;
            publicPageDTO.ClaimsStat1Icon = publicPage.ClaimsStat1Icon;
            publicPageDTO.ClaimsStat1Value = publicPage.ClaimsStat1Value;
            publicPageDTO.ClaimsStat1Label = publicPage.ClaimsStat1Label;
            publicPageDTO.ClaimsStat2Icon = publicPage.ClaimsStat2Icon;
            publicPageDTO.ClaimsStat2Value = publicPage.ClaimsStat2Value;
            publicPageDTO.ClaimsStat2Label = publicPage.ClaimsStat2Label;
            publicPageDTO.ClaimsStat3Icon = publicPage.ClaimsStat3Icon;
            publicPageDTO.ClaimsStat3Value = publicPage.ClaimsStat3Value;
            publicPageDTO.ClaimsStat3Label = publicPage.ClaimsStat3Label;
            publicPageDTO.ClaimsTableHeadersJson = publicPage.ClaimsTableHeadersJson;
            publicPageDTO.ClaimsYearsRange = publicPage.ClaimsYearsRange;

            /* ========== CONTACT US ========== */
            publicPageDTO.ContactHeaderTitle = publicPage.ContactHeaderTitle;
            publicPageDTO.ContactHeaderSubTitle = publicPage.ContactHeaderSubTitle;
            publicPageDTO.ContactFullNameLabel = publicPage.ContactFullNameLabel;
            publicPageDTO.ContactPhoneLabel = publicPage.ContactPhoneLabel;
            publicPageDTO.ContactEmailLabel = publicPage.ContactEmailLabel;
            publicPageDTO.ContactSubjectLabel = publicPage.ContactSubjectLabel;
            publicPageDTO.ContactMessageLabel = publicPage.ContactMessageLabel;
            publicPageDTO.ContactSubmitButtonLabel = publicPage.ContactSubmitButtonLabel;
            publicPageDTO.OfficeTitle = publicPage.OfficeTitle;
            publicPageDTO.OfficeAddress = publicPage.OfficeAddress;
            publicPageDTO.OfficePhone = publicPage.OfficePhone;
            publicPageDTO.OfficeEmail = publicPage.OfficeEmail;
            publicPageDTO.OfficeHoursTitle = publicPage.OfficeHoursTitle;
            publicPageDTO.OfficeDay1Time = publicPage.OfficeDay1Time;
            publicPageDTO.OfficeDay2Time = publicPage.OfficeDay2Time;
            publicPageDTO.OfficeDay3Time = publicPage.OfficeDay3Time;

            /* ========== FOOTER ========== */
            publicPageDTO.FooterBrandShortName = publicPage.FooterBrandShortName;
            publicPageDTO.FooterBrandSubTitle = publicPage.FooterBrandSubTitle;
            publicPageDTO.FooterBrandDescription = publicPage.FooterBrandDescription;
            publicPageDTO.FooterLogoAlt = publicPage.FooterLogoAlt;
            publicPageDTO.FooterAddressLine1 = publicPage.FooterAddressLine1;
            publicPageDTO.FooterAddressLine2 = publicPage.FooterAddressLine2;
            publicPageDTO.FooterPhoneIcon = publicPage.FooterPhoneIcon;
            publicPageDTO.FooterPhoneValue = publicPage.FooterPhoneValue;
            publicPageDTO.FooterEmailIcon = publicPage.FooterEmailIcon;
            publicPageDTO.FooterEmailValue = publicPage.FooterEmailValue;
            publicPageDTO.FooterQuickLinksJson = publicPage.FooterQuickLinksJson;
            publicPageDTO.FooterOfficeHoursJson = publicPage.FooterOfficeHoursJson;
            publicPageDTO.FooterQuickHead = publicPage.FooterQuickHead;
            publicPageDTO.FooterCopyrightText = publicPage.FooterCopyrightText;

            /* ========== PRIVACY POLICY ========== */
            publicPageDTO.PrivacyHeroBadge = publicPage.PrivacyHeroBadge;
            publicPageDTO.PrivacyHeroTitle = publicPage.PrivacyHeroTitle;
            publicPageDTO.PrivacyHeroSubTitle = publicPage.PrivacyHeroSubTitle;
            publicPageDTO.PrivacyHeading1 = publicPage.PrivacyHeading1;
            publicPageDTO.PrivacyPara1 = publicPage.PrivacyPara1;
            publicPageDTO.PrivacyPara2 = publicPage.PrivacyPara2;
            publicPageDTO.PrivacyParagraph3 = publicPage.PrivacyParagraph3;
            publicPageDTO.PrivacyHeading2 = publicPage.PrivacyHeading2;
            publicPageDTO.PrivacyPara3 = publicPage.PrivacyPara3;
            publicPageDTO.PrivacyHeading3 = publicPage.PrivacyHeading3;
            publicPageDTO.PrivacyLine1 = publicPage.PrivacyLine1;
            publicPageDTO.PrivacyLine2 = publicPage.PrivacyLine2;
            publicPageDTO.PrivacyLine3 = publicPage.PrivacyLine3;
            publicPageDTO.PrivacyLine4 = publicPage.PrivacyLine4;
            publicPageDTO.PrivacyLine5 = publicPage.PrivacyLine5;
            publicPageDTO.PrivacyLine6 = publicPage.PrivacyLine6;
            publicPageDTO.PrivacyHeading3Para1 = publicPage.PrivacyHeading3Para1;
            publicPageDTO.PrivacyHeading4 = publicPage.PrivacyHeading4;
            publicPageDTO.PrivacySubHeading4 = publicPage.PrivacySubHeading4;
            publicPageDTO.PrivacyLine7 = publicPage.PrivacyLine7;
            publicPageDTO.PrivacyHeading5 = publicPage.PrivacyHeading5;
            publicPageDTO.PrivacyHeading5Para1 = publicPage.PrivacyHeading5Para1;
            publicPageDTO.PrivacyHeading6 = publicPage.PrivacyHeading6;
            publicPageDTO.PrivacyHeading6Para1 = publicPage.PrivacyHeading6Para1;
            publicPageDTO.PrivacyHeading7 = publicPage.PrivacyHeading7;
            publicPageDTO.PrivacyHeading7Para1 = publicPage.PrivacyHeading7Para1;
            publicPageDTO.PrivacyHeading8 = publicPage.PrivacyHeading8;
            publicPageDTO.PrivacySubHeading8 = publicPage.PrivacySubHeading8;
            publicPageDTO.PrivacyHeading8Para1 = publicPage.PrivacyHeading8Para1;
            publicPageDTO.PrivacyHeading8Para2 = publicPage.PrivacyHeading8Para2;
            publicPageDTO.PrivacyHeading8Para3 = publicPage.PrivacyHeading8Para3;
            publicPageDTO.PrivacyHeading8Para4 = publicPage.PrivacyHeading8Para4;
            publicPageDTO.PrivacyHeading9 = publicPage.PrivacyHeading9;
            publicPageDTO.PrivacySubHeading9 = publicPage.PrivacySubHeading9;
            publicPageDTO.PrivacyHeading9Para1 = publicPage.PrivacyHeading9Para1;
            publicPageDTO.PrivacyHeading9Para2 = publicPage.PrivacyHeading9Para2;
            publicPageDTO.PrivacyHeading9Para3 = publicPage.PrivacyHeading9Para3;
            publicPageDTO.PrivacyHeading9Para4 = publicPage.PrivacyHeading9Para4;
            publicPageDTO.PrivacyHeading9Para5 = publicPage.PrivacyHeading9Para5;
            publicPageDTO.PrivacyHeading9Para6 = publicPage.PrivacyHeading9Para6;
            publicPageDTO.PrivacyHeading9Para7 = publicPage.PrivacyHeading9Para7;
            publicPageDTO.PrivacyHeading10 = publicPage.PrivacyHeading10;
            publicPageDTO.PrivacyHeading10Para1 = publicPage.PrivacyHeading10Para1;
            publicPageDTO.PrivacyHeading10Para2 = publicPage.PrivacyHeading10Para2;
            publicPageDTO.PrivacyHeading11 = publicPage.PrivacyHeading11;
            publicPageDTO.PrivacyHeading11Para1 = publicPage.PrivacyHeading11Para1;
            publicPageDTO.PrivacyHeading11Para2 = publicPage.PrivacyHeading11Para2;
            publicPageDTO.PrivacyHeading12 = publicPage.PrivacyHeading12;
            publicPageDTO.PrivacyHeading12Para1 = publicPage.PrivacyHeading12Para1;

            /* ========== CONTACT FORM PLACEHOLDERS ========== */
            publicPageDTO.ContactFullNamePlaceholder = publicPage.ContactFullNamePlaceholder;
            publicPageDTO.ContactPhoneNumberPlaceholder = publicPage.ContactPhoneNumberPlaceholder;
            publicPageDTO.ContactEmailPlaceholder = publicPage.ContactEmailPlaceholder;
            publicPageDTO.ContactSubjectPlaceholder = publicPage.ContactSubjectPlaceholder;
            publicPageDTO.ContactMessagePlaceholder = publicPage.ContactMessagePlaceholder;
            publicPageDTO.ContactMessageRowNo = publicPage.ContactMessageRowNo;
            publicPageDTO.ContactSubmitButtonIconClass = publicPage.ContactSubmitButtonIconClass;

            /* ========== CONTACT OFFICE DETAILS ========== */
            publicPageDTO.ContactOfficeTitleLabel = publicPage.ContactOfficeTitleLabel;
            publicPageDTO.ContactOfficeTitleIconClass = publicPage.ContactOfficeTitleIconClass;
            publicPageDTO.ContactOfficePhoneLabel = publicPage.ContactOfficePhoneLabel;
            publicPageDTO.ContactOfficePhoneIconClass = publicPage.ContactOfficePhoneIconClass;
            publicPageDTO.ContactOfficeEmailLabel = publicPage.ContactOfficeEmailLabel;
            publicPageDTO.ContactOfficeEmailIconClass = publicPage.ContactOfficeEmailIconClass;
            publicPageDTO.ContactOfficeAddress2 = publicPage.ContactOfficeAddress2;
            publicPageDTO.ContactOfficeAddress3 = publicPage.ContactOfficeAddress3;
            publicPageDTO.ContactOfficeDay1 = publicPage.ContactOfficeDay1;
            publicPageDTO.ContactOfficeDay2 = publicPage.ContactOfficeDay2;
            publicPageDTO.ContactOfficeDay3 = publicPage.ContactOfficeDay3;

            publicPageDTO.IsActive = publicPage.IsActive;

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
               changedBy: "System"
           );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<PublicPageDTO>> GetAllAsync()
        {
            var publicPages = _repo.GetQueryablePublicPageList();
            return await publicPages.ToListAsync();
        }

        public async Task<PublicPageDTO?> GetByIdAsync(int id)
        {
            var publicPage = await _repo.GetByIdAsync(id);
            if (publicPage == null) return null;
            return ConvertPublicToDTO(publicPage);
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
               changedBy: "System"
           );
            return true;
        }
    }
}