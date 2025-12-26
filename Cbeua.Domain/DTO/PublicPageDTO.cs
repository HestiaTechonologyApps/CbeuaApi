using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class PublicPageDTO
    {
        public int PublicPageId { get; set; }

        /* ===== NAVBAR ===== */
        public string NavbarTitle { get; set; } = "";
        public string NavbarSubTitle { get; set; } = "";
        public string NavbarLogoAlt { get; set; } = "";
        public string NavbarMenuHead { get; set; } = "";

        public string NavbarMenuJson { get; set; } = "";

        public string NavbarLoginLabel { get; set; } = "";
        public string NavbarLoginIconClass { get; set; } = "";

        public string NavbarPhone { get; set; } = "";
        public string NavbarEmail { get; set; } = "";

        /* ===== HERO ===== */
        public string HeroBadge { get; set; } = "";
        public string HeroTitleLine1 { get; set; } = "";
        public string HeroTitleHighlight { get; set; } = "";
        public string HeroTitleLine3 { get; set; } = "";
        public string HeroDescription { get; set; } = "";

        public string HeroPrimaryBtnLabel { get; set; } = "";
        public string HeroPrimaryBtnRoute { get; set; } = "";
        public string HeroSecondaryBtnLabel { get; set; } = "";
        public string HeroSecondaryBtnRoute { get; set; } = "";

        /* ===== FEATURES ===== */
        public string FeaturesHeadingLabel { get; set; } = "";
        public string FeaturesHeadingTitle { get; set; } = "";
        public string FeaturesHeadingSubtitle { get; set; } = "";

        public string FeaturesItemsJson { get; set; } = "";

        /* ===== FOOTER ===== */
        public string FooterShortName { get; set; } = "";
        public string FooterSubtitle { get; set; } = "";
        public string FooterDescription { get; set; } = "";

        public string FooterAddress1 { get; set; } = "";
        public string FooterAddress2 { get; set; } = "";
        public string FooterPhone { get; set; } = "";
        public string FooterEmail { get; set; } = "";

        public string FooterQuickLinksJson { get; set; } = "";
        public string FooterOfficeHoursJson { get; set; } = "";
        public string FooterBottomBarText { get; set; } = "";

       
    }
}
