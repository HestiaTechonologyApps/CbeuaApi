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

                    PrivacyHeading2 = wc.PrivacyHeading2,
                    PrivacyPara3 = wc.PrivacyPara3,

                    PrivacyHeading3 = wc.PrivacyHeading3,
                    PrivacyLine1 = wc.PrivacyLine1,
                    PrivacyLine2 = wc.PrivacyLine2,
                    PrivacyLine3 = wc.PrivacyLine3,
                    PrivacyLine4 = wc.PrivacyLine4,
                    PrivacyLine5 = wc.PrivacyLine5,
                    PrivacyLine6 = wc.PrivacyLine6

                });
        }
    }
}
