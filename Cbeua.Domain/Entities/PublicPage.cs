using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities
{
    public class PublicPage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PublicPageId { get; set; }

        public string NavBrandTitle { get; set; } = "";
        public string NavBrandSubTitle { get; set; } = "";
        public string NavLogoUrl { get; set; } = "";
        public string NavLogoAlt { get; set; } = "";
        public bool NavMenuHead { get; set; }

        public string NavHomeLabel { get; set; } = "";
        public string NavAboutLabel { get; set; } = "";
        public string NavRulesLabel { get; set; } = "";
        public string NavDownloadsLabel { get; set; } = "";
        public string NavCommitteeLabel { get; set; } = "";
        public string NavClaimsLabel { get; set; } = "";
        public string NavContactLabel { get; set; } = "";

        public string NavLoginLabel { get; set; } = "";
        public string NavLoginIcon { get; set; } = "";

        public string NavPhoneIcon { get; set; } = "";
        public string NavPhoneValue { get; set; } = "";
        public string NavEmailIcon { get; set; } = "";
        public string NavEmailValue { get; set; } = "";

        /* ================= HOME PAGE ================= */
        public string HomeHeroBadge { get; set; } = "";
        public string HomeHeroTitle { get; set; } = "";
        public string HomeHeroLine1 { get; set; } = "";
        public string HomeHeroHighlight { get; set; } = "";
        public string HomeHeroLine3 { get; set; } = "";
        public string HomeHeroDescription { get; set; } = "";

        public string HomePrimaryBtnLabel { get; set; } = "";
        public string HomePrimaryBtnRoute { get; set; } = "";
        public string HomeSecondaryBtnLabel { get; set; } = "";
        public string HomeSecondaryBtnRoute { get; set; } = "";

        public string HomeHeroImageUrl { get; set; } = "";
        public string HomeHeroImageAlt { get; set; } = "";

        public string HomeFeatureHeading { get; set; } = "";
        public string HomeFeatureLabel { get; set; } = "";
        public string HomeFeatureTitle { get; set; } = "";
        public string HomeFeatureSubTitle { get; set; } = "";
        public string HomeFeatureItemsJson { get; set; } = ""; // icon,title,description[]

        public string HomeAboutLabel { get; set; } = "";
        public string HomeAboutTitle { get; set; } = "";
        public string HomeAboutParagraph { get; set; } = "";

        /* ================= NEWS PAGE ================= */
        public string NewsHeroTitle { get; set; } = "";
        public string NewsHeroSubTitle { get; set; } = "";
        public string NewsBreadcrumbHomeLabel { get; set; } = "";
        public string NewsBreadcrumbCurrentLabel { get; set; } = "";
        public string NewsLoadingText { get; set; } = "";
        public string NewsEmptyText { get; set; } = "";
        public string NewsItemsJson { get; set; } = ""; // date,title,excerpt[]
        public string NewsSidebarQuoteTitle { get; set; } = "";
        public string NewsSidebarQuoteText { get; set; } = "";
        public string NewsQuickLinksJson { get; set; } = ""; // labels[]

        /* ================= ABOUT PAGE ================= */
        public string AboutHeaderTitle { get; set; } = "";
        public string AboutHeaderSubTitle { get; set; } = "";

        public string AboutMissionTitle { get; set; } = "";
        public string AboutMissionIcon { get; set; } = "";
        public string AboutMissionDescription { get; set; } = "";

        public string AboutVisionTitle { get; set; } = "";
        public string AboutVisionIcon { get; set; } = "";
        public string AboutVisionDescription { get; set; } = "";

        public string AboutHistoryTitle { get; set; } = "";
        public string AboutHistoryIcon { get; set; } = "";
        public string AboutHistoryPara1 { get; set; } = "";
        public string AboutHistoryPara2 { get; set; } = "";
        public string AboutHistoryPara3 { get; set; } = "";
        public string AboutHistoryPara4 { get; set; } = "";
        public string AboutHistoryPara5 { get; set; } = "";

        /* ================= RULES & REGULATIONS ================= */
        public string RulesHeaderTitle { get; set; } = "";
        public string RulesHeaderSubTitle { get; set; } = "";

        public string RulesPreambleTitle { get; set; } = "";
        public string RulesPreamblePara1 { get; set; } = "";
        public string RulesPreamblePara2 { get; set; } = "";
        public string RulesPreamblePara3 { get; set; } = "";
        public string RulesPreamblePara4 { get; set; } = "";
        public string RulesPreamblePara5 { get; set; } = "";

        public string RulesSectionsJson { get; set; } = ""; // number,title,content/list[]

        /* ================= DOWNLOADS ================= */
        public string DownloadsHeaderTitle { get; set; } = "";
        public string DownloadsHeaderSubTitle { get; set; } = "";
        public string DownloadItemsJson { get; set; } = ""; // title,icon,fileKey,description[]

        /* ================= MANAGING COMMITTEE ================= */
        public string CommitteeHeaderTitle { get; set; } = "";
        public string CommitteeHeaderSubTitle { get; set; } = "";
        public string CommitteeMembersJson { get; set; } = ""; // name,role,location,phone,email[]

        /* ================= CLAIMS PAGE ================= */
        public string ClaimsHeroTitle { get; set; } = "";
        public string ClaimsHeroSubTitle { get; set; } = "";

        public string ClaimsStat1Icon { get; set; } = "";
        public string ClaimsStat1Value { get; set; } = "";
        public string ClaimsStat1Label { get; set; } = "";

        public string ClaimsStat2Icon { get; set; } = "";
        public string ClaimsStat2Value { get; set; } = "";
        public string ClaimsStat2Label { get; set; } = "";

        public string ClaimsStat3Icon { get; set; } = "";
        public string ClaimsStat3Value { get; set; } = "";
        public string ClaimsStat3Label { get; set; } = "";

        public string ClaimsTableHeadersJson { get; set; } = ""; // name,total,year[]
        public string ClaimsYearsRange { get; set; } = ""; // 2002-2020

        /* ================= CONTACT US ================= */
        public string ContactHeaderTitle { get; set; } = "";
        public string ContactHeaderSubTitle { get; set; } = "";

        public string ContactFullNameLabel { get; set; } = "";
        public string ContactPhoneLabel { get; set; } = "";
        public string ContactEmailLabel { get; set; } = "";
        public string ContactSubjectLabel { get; set; } = "";
        public string ContactMessageLabel { get; set; } = "";
        public string ContactSubmitButtonLabel { get; set; } = "";

        public string OfficeTitle { get; set; } = "";
        public string OfficeAddress { get; set; } = "";
        public string OfficePhone { get; set; } = "";
        public string OfficeEmail { get; set; } = "";

        public string OfficeHoursTitle { get; set; } = "";
        public string OfficeDay1Time { get; set; } = "";
        public string OfficeDay2Time { get; set; } = "";
        public string OfficeDay3Time { get; set; } = "";

        /* ================= FOOTER ================= */
        public string FooterBrandShortName { get; set; } = "";
        public string FooterBrandSubTitle { get; set; } = "";
        public string FooterBrandDescription { get; set; } = "";
        public string FooterLogoAlt { get; set; } = "";

        public string FooterAddressLine1 { get; set; } = "";
        public string FooterAddressLine2 { get; set; } = "";

        public string FooterPhoneIcon { get; set; } = "";
        public string FooterPhoneValue { get; set; } = "";

        public string FooterEmailIcon { get; set; } = "";
        public string FooterEmailValue { get; set; } = "";

        public string FooterQuickLinksJson { get; set; } = ""; // labels[]
        public string FooterOfficeHoursJson { get; set; } = ""; // weekdays,time[]

        public string FooterCopyrightText { get; set; } = "";

        /* ================= PRIVACY POLICY ================= */
        public string PrivacyHeroBadge { get; set; } = "";
        public string PrivacyHeroTitle { get; set; } = "";
        public string PrivacyHeroSubTitle { get; set; } = "";

        public string PrivacyHeading1 { get; set; } = "";
        public string PrivacyPara1 { get; set; } = "";
        public string PrivacyPara2 { get; set; } = "";

        public string PrivacyHeading2 { get; set; } = "";
        public string PrivacyPara3 { get; set; } = "";

        public string PrivacyHeading3 { get; set; } = "";
        public string PrivacyLine1 { get; set; } = "";
        public string PrivacyLine2 { get; set; } = "";
        public string PrivacyLine3 { get; set; } = "";
        public string PrivacyLine4 { get; set; } = "";
        public string PrivacyLine5 { get; set; } = "";
        public string PrivacyLine6 { get; set; } = "";

        public bool IsActive { get; set; } = true;
    }


}

