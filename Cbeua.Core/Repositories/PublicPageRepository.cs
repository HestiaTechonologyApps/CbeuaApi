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
                .Select(pp => new PublicPageDTO
                {
                    PublicPageId = pp.PublicPageId,
                    NavbarTitle = pp.NavbarTitle,
                    NavbarSubTitle = pp.NavbarSubTitle,
                    NavbarLogoAlt = pp.NavbarLogoAlt,
                    NavbarMenuHead = pp.NavbarMenuHead,
                    NavbarMenuJson = pp.NavbarMenuJson,
                    NavbarLoginIconClass = pp.NavbarLoginIconClass,
                    NavbarLoginLabel = pp.NavbarLoginLabel,
                    NavbarPhone = pp.NavbarPhone,
                    NavbarEmail = pp.NavbarEmail,
                    HeroBadge = pp.HeroBadge,
                    HeroTitleLine1 = pp.HeroTitleLine1,
                    HeroTitleHighlight = pp.HeroTitleHighlight,
                    HeroTitleLine3 = pp.HeroTitleLine3,
                    HeroDescription = pp.HeroDescription,
                    HeroPrimaryBtnLabel = pp.HeroPrimaryBtnLabel,
                    HeroPrimaryBtnRoute = pp.HeroPrimaryBtnRoute,
                    HeroSecondaryBtnLabel = pp.HeroSecondaryBtnLabel,

                    HeroSecondaryBtnRoute = pp.HeroSecondaryBtnRoute,
                    FeaturesHeadingLabel = pp.FeaturesHeadingLabel,
                    FeaturesHeadingTitle = pp.FeaturesHeadingTitle,
                    FeaturesHeadingSubtitle = pp.FeaturesHeadingSubtitle,
                    FeaturesItemsJson = pp.FeaturesItemsJson,
                    FooterSubtitle = pp.FooterSubtitle,
                    FooterShortName = pp.FooterShortName,
                    FooterDescription = pp.FooterDescription,
                    FooterAddress1 = pp.FooterAddress1,
                    FooterAddress2 = pp.FooterAddress2,
                    FooterPhone = pp.FooterPhone,
                    FooterEmail = pp.FooterEmail,
                    FooterQuickLinksJson = pp.FooterQuickLinksJson,
                    FooterOfficeHoursJson = pp.FooterOfficeHoursJson,
                    FooterBottomBarText = pp.FooterBottomBarText
                    




                });
        }
    }
}
