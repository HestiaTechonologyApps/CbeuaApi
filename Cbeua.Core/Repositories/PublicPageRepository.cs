using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class PublicPageRepository : GenericRepository<PublicPage>, IPublicPageRepository
    {
        private readonly AppDbContext _context;
        public PublicPageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<PublicPageDTO> GetQueryablePublicPageList()
        {
            return _context.publicPages
                .Select(wc => new PublicPageDTO
                {
                    PublicPageId = wc.PublicPageId,

                    /* ========== NAVBAR ========== */
                    NavBrandTitle = wc.NavBrandTitle,
                    NavBrandSubTitle = wc.NavBrandSubTitle,
                    NavLogoUrl = wc.NavLogoUrl,
                    NavLogoAlt = wc.NavLogoAlt,
                    NavMenuHead = wc.NavMenuHead,

                    NavHomeLabel = wc.NavHomeLabel,
                    NavAboutLabel = wc.NavAboutLabel,
                    NavRulesLabel = wc.NavRulesLabel,
                    NavDownloadsLabel = wc.NavDownloadsLabel,
                    NavCommitteeLabel = wc.NavCommitteeLabel,
                    NavClaimsLabel = wc.NavClaimsLabel,
                    NavContactLabel = wc.NavContactLabel,

                    NavLoginLabel = wc.NavLoginLabel,
                    NavLoginIcon = wc.NavLoginIcon,

                    NavPhoneIcon = wc.NavPhoneIcon,
                    NavPhoneValue = wc.NavPhoneValue,
                    NavEmailIcon = wc.NavEmailIcon,
                    NavEmailValue = wc.NavEmailValue,

                    /* ========== HOME – HERO ========== */
                    HomeHeroBadge = wc.HomeHeroBadge,
                    HomeHeroTitle = wc.HomeHeroTitle,
                    HomeHeroLine1 = wc.HomeHeroLine1,
                    HomeHeroHighlight = wc.HomeHeroHighlight,
                    HomeHeroLine3 = wc.HomeHeroLine3,
                    HomeHeroDescription = wc.HomeHeroDescription,

                    HomePrimaryBtnLabel = wc.HomePrimaryBtnLabel,
                    HomePrimaryBtnRoute = wc.HomePrimaryBtnRoute,
                    HomeSecondaryBtnLabel = wc.HomeSecondaryBtnLabel,
                    HomeSecondaryBtnRoute = wc.HomeSecondaryBtnRoute,

                    HomeHeroImageUrl = wc.HomeHeroImageUrl,
                    HomeHeroImageAlt = wc.HomeHeroImageAlt,

                    /* ========== HOME – FEATURES ========== */
                    HomeFeatureHeading = wc.HomeFeatureHeading,
                    HomeFeatureLabel = wc.HomeFeatureLabel,
                    HomeFeatureTitle = wc.HomeFeatureTitle,
                    HomeFeatureSubTitle = wc.HomeFeatureSubTitle,
                    HomeFeatureItemsJson = wc.HomeFeatureItemsJson,

                    /* ========== HOME – ABOUT ========== */
                    HomeAboutLabel = wc.HomeAboutLabel,
                    HomeAboutTitle = wc.HomeAboutTitle,
                    HomeAboutParagraph = wc.HomeAboutParagraph,

                    /* ========== NEWS PAGE ========== */
                    NewsHeroTitle = wc.NewsHeroTitle,
                    NewsHeroSubTitle = wc.NewsHeroSubTitle,
                    NewsBreadcrumbHomeLabel = wc.NewsBreadcrumbHomeLabel,
                    NewsBreadcrumbCurrentLabel = wc.NewsBreadcrumbCurrentLabel,
                    NewsLoadingText = wc.NewsLoadingText,
                    NewsEmptyText = wc.NewsEmptyText,
                    NewsItemsJson = wc.NewsItemsJson,
                    NewsSidebarQuoteTitle = wc.NewsSidebarQuoteTitle,
                    NewsSidebarQuoteText = wc.NewsSidebarQuoteText,
                    NewsQuickLinksJson = wc.NewsQuickLinksJson,

                    /* ========== ABOUT PAGE ========== */
                    AboutHeaderTitle = wc.AboutHeaderTitle,
                    AboutHeaderSubTitle = wc.AboutHeaderSubTitle,
                    AboutMissionTitle = wc.AboutMissionTitle,
                    AboutMissionIcon = wc.AboutMissionIcon,
                    AboutMissionDescription = wc.AboutMissionDescription,
                    AboutVisionTitle = wc.AboutVisionTitle,
                    AboutVisionIcon = wc.AboutVisionIcon,
                    AboutVisionDescription = wc.AboutVisionDescription,
                    AboutHistoryTitle = wc.AboutHistoryTitle,
                    AboutHistoryIcon = wc.AboutHistoryIcon,
                    AboutHistoryPara1 = wc.AboutHistoryPara1,
                    AboutHistoryPara2 = wc.AboutHistoryPara2,
                    AboutHistoryPara3 = wc.AboutHistoryPara3,
                    AboutHistoryPara4 = wc.AboutHistoryPara4,
                    AboutHistoryPara5 = wc.AboutHistoryPara5,

                    /* ========== RULES & REGULATIONS ========== */
                    RulesHeaderTitle = wc.RulesHeaderTitle,
                    RulesHeaderSubTitle = wc.RulesHeaderSubTitle,
                    RulesPreambleTitle = wc.RulesPreambleTitle,
                    RulesPreamblePara1 = wc.RulesPreamblePara1,
                    RulesPreamblePara2 = wc.RulesPreamblePara2,
                    RulesPreamblePara3 = wc.RulesPreamblePara3,
                    RulesPreamblePara4 = wc.RulesPreamblePara4,
                    RulesPreamblePara5 = wc.RulesPreamblePara5,
                    RulesSectionsJson = wc.RulesSectionsJson,

                    /* ========== DOWNLOADS ========== */
                    DownloadsHeaderTitle = wc.DownloadsHeaderTitle,
                    DownloadsHeaderSubTitle = wc.DownloadsHeaderSubTitle,
                    DownloadItemsJson = wc.DownloadItemsJson,

                    /* ========== MANAGING COMMITTEE ========== */
                    CommitteeHeaderTitle = wc.CommitteeHeaderTitle,
                    CommitteeHeaderSubTitle = wc.CommitteeHeaderSubTitle,
                    CommitteeMembersJson = wc.CommitteeMembersJson,

                    /* ========== CLAIMS PAGE ========== */
                    ClaimsHeroTitle = wc.ClaimsHeroTitle,
                    ClaimsHeroSubTitle = wc.ClaimsHeroSubTitle,
                    ClaimsStat1Icon = wc.ClaimsStat1Icon,
                    ClaimsStat1Value = wc.ClaimsStat1Value,
                    ClaimsStat1Label = wc.ClaimsStat1Label,
                    ClaimsStat2Icon = wc.ClaimsStat2Icon,
                    ClaimsStat2Value = wc.ClaimsStat2Value,
                    ClaimsStat2Label = wc.ClaimsStat2Label,
                    ClaimsStat3Icon = wc.ClaimsStat3Icon,
                    ClaimsStat3Value = wc.ClaimsStat3Value,
                    ClaimsStat3Label = wc.ClaimsStat3Label,
                    ClaimsTableHeadersJson = wc.ClaimsTableHeadersJson,
                    ClaimsYearsRange = wc.ClaimsYearsRange,

                    /* ========== CONTACT US ========== */
                    ContactHeaderTitle = wc.ContactHeaderTitle,
                    ContactHeaderSubTitle = wc.ContactHeaderSubTitle,
                    ContactFullNameLabel = wc.ContactFullNameLabel,
                    ContactPhoneLabel = wc.ContactPhoneLabel,
                    ContactEmailLabel = wc.ContactEmailLabel,
                    ContactSubjectLabel = wc.ContactSubjectLabel,
                    ContactMessageLabel = wc.ContactMessageLabel,
                    ContactSubmitButtonLabel = wc.ContactSubmitButtonLabel,
                    OfficeTitle = wc.OfficeTitle,
                    OfficeAddress = wc.OfficeAddress,
                    OfficePhone = wc.OfficePhone,
                    OfficeEmail = wc.OfficeEmail,
                    OfficeHoursTitle = wc.OfficeHoursTitle,
                    OfficeDay1Time = wc.OfficeDay1Time,
                    OfficeDay2Time = wc.OfficeDay2Time,
                    OfficeDay3Time = wc.OfficeDay3Time,

                    /* ========== FOOTER ========== */
                    FooterBrandShortName = wc.FooterBrandShortName,
                    FooterBrandSubTitle = wc.FooterBrandSubTitle,
                    FooterBrandDescription = wc.FooterBrandDescription,
                    FooterLogoAlt = wc.FooterLogoAlt,
                    FooterAddressLine1 = wc.FooterAddressLine1,
                    FooterAddressLine2 = wc.FooterAddressLine2,
                    FooterPhoneIcon = wc.FooterPhoneIcon,
                    FooterPhoneValue = wc.FooterPhoneValue,
                    FooterEmailIcon = wc.FooterEmailIcon,
                    FooterEmailValue = wc.FooterEmailValue,
                    FooterQuickLinksJson = wc.FooterQuickLinksJson,
                    FooterOfficeHoursJson = wc.FooterOfficeHoursJson,
                    FooterCopyrightText = wc.FooterCopyrightText,

                    /* ========== PRIVACY POLICY ========== */
                    PrivacyHeroBadge = wc.PrivacyHeroBadge,
                    PrivacyHeroTitle = wc.PrivacyHeroTitle,
                    PrivacyHeroSubTitle = wc.PrivacyHeroSubTitle,
                    PrivacyHeading1 = wc.PrivacyHeading1,
                    PrivacyPara1 = wc.PrivacyPara1,
                    PrivacyPara2 = wc.PrivacyPara2,
                    PrivacyParagraph3 = wc.PrivacyParagraph3,
                    PrivacyHeading2 = wc.PrivacyHeading2,
                    PrivacyPara3 = wc.PrivacyPara3,
                    PrivacyHeading3 = wc.PrivacyHeading3,
                    PrivacyLine1 = wc.PrivacyLine1,
                    PrivacyLine2 = wc.PrivacyLine2,
                    PrivacyLine3 = wc.PrivacyLine3,
                    PrivacyLine4 = wc.PrivacyLine4,
                    PrivacyLine5 = wc.PrivacyLine5,
                    PrivacyLine6 = wc.PrivacyLine6,

                    IsActive = wc.IsActive
                });
        }
    }
}
