using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedchangepublicpagefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NavbarTitle",
                table: "publicPages",
                newName: "RulesSectionsJson");

            migrationBuilder.RenameColumn(
                name: "NavbarSubTitle",
                table: "publicPages",
                newName: "RulesPreambleTitle");

            migrationBuilder.RenameColumn(
                name: "NavbarPhone",
                table: "publicPages",
                newName: "RulesPreamblePara5");

            migrationBuilder.RenameColumn(
                name: "NavbarMenuJson",
                table: "publicPages",
                newName: "RulesPreamblePara4");

            migrationBuilder.RenameColumn(
                name: "NavbarMenuHead",
                table: "publicPages",
                newName: "RulesPreamblePara3");

            migrationBuilder.RenameColumn(
                name: "NavbarLogoAlt",
                table: "publicPages",
                newName: "RulesPreamblePara2");

            migrationBuilder.RenameColumn(
                name: "NavbarLoginLabel",
                table: "publicPages",
                newName: "RulesPreamblePara1");

            migrationBuilder.RenameColumn(
                name: "NavbarLoginIconClass",
                table: "publicPages",
                newName: "RulesHeaderTitle");

            migrationBuilder.RenameColumn(
                name: "NavbarEmail",
                table: "publicPages",
                newName: "RulesHeaderSubTitle");

            migrationBuilder.RenameColumn(
                name: "HeroTitleLine3",
                table: "publicPages",
                newName: "PrivacyPara3");

            migrationBuilder.RenameColumn(
                name: "HeroTitleLine1",
                table: "publicPages",
                newName: "PrivacyPara2");

            migrationBuilder.RenameColumn(
                name: "HeroTitleHighlight",
                table: "publicPages",
                newName: "PrivacyPara1");

            migrationBuilder.RenameColumn(
                name: "HeroSecondaryBtnRoute",
                table: "publicPages",
                newName: "PrivacyLine6");

            migrationBuilder.RenameColumn(
                name: "HeroSecondaryBtnLabel",
                table: "publicPages",
                newName: "PrivacyLine5");

            migrationBuilder.RenameColumn(
                name: "HeroPrimaryBtnRoute",
                table: "publicPages",
                newName: "PrivacyLine4");

            migrationBuilder.RenameColumn(
                name: "HeroPrimaryBtnLabel",
                table: "publicPages",
                newName: "PrivacyLine3");

            migrationBuilder.RenameColumn(
                name: "HeroDescription",
                table: "publicPages",
                newName: "PrivacyLine2");

            migrationBuilder.RenameColumn(
                name: "HeroBadge",
                table: "publicPages",
                newName: "PrivacyLine1");

            migrationBuilder.RenameColumn(
                name: "FooterSubtitle",
                table: "publicPages",
                newName: "PrivacyHeroTitle");

            migrationBuilder.RenameColumn(
                name: "FooterShortName",
                table: "publicPages",
                newName: "PrivacyHeroSubTitle");

            migrationBuilder.RenameColumn(
                name: "FooterPhone",
                table: "publicPages",
                newName: "PrivacyHeroBadge");

            migrationBuilder.RenameColumn(
                name: "FooterEmail",
                table: "publicPages",
                newName: "PrivacyHeading3");

            migrationBuilder.RenameColumn(
                name: "FooterDescription",
                table: "publicPages",
                newName: "PrivacyHeading2");

            migrationBuilder.RenameColumn(
                name: "FooterBottomBarText",
                table: "publicPages",
                newName: "PrivacyHeading1");

            migrationBuilder.RenameColumn(
                name: "FooterAddress2",
                table: "publicPages",
                newName: "OfficeTitle");

            migrationBuilder.RenameColumn(
                name: "FooterAddress1",
                table: "publicPages",
                newName: "OfficePhone");

            migrationBuilder.RenameColumn(
                name: "FeaturesItemsJson",
                table: "publicPages",
                newName: "OfficeHoursTitle");

            migrationBuilder.RenameColumn(
                name: "FeaturesHeadingTitle",
                table: "publicPages",
                newName: "OfficeEmail");

            migrationBuilder.RenameColumn(
                name: "FeaturesHeadingSubtitle",
                table: "publicPages",
                newName: "OfficeDay3Time");

            migrationBuilder.RenameColumn(
                name: "FeaturesHeadingLabel",
                table: "publicPages",
                newName: "OfficeDay2Time");

            migrationBuilder.AddColumn<string>(
                name: "AboutHeaderSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHeaderTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHistoryIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHistoryPara1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHistoryPara2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHistoryPara3",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHistoryPara4",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHistoryPara5",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutHistoryTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutMissionDescription",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutMissionIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutMissionTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutVisionDescription",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutVisionIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutVisionTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsHeroSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsHeroTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat1Icon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat1Label",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat1Value",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat2Icon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat2Label",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat2Value",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat3Icon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat3Label",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsStat3Value",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsTableHeadersJson",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimsYearsRange",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommitteeHeaderSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommitteeHeaderTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMembersJson",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmailLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactFullNameLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactHeaderSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactHeaderTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactMessageLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhoneLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactSubjectLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactSubmitButtonLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadItemsJson",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadsHeaderSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadsHeaderTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterAddressLine1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterAddressLine2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterBrandDescription",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterBrandShortName",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterBrandSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterCopyrightText",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterEmailIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterEmailValue",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterLogoAlt",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterPhoneIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterPhoneValue",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeAboutLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeAboutParagraph",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeAboutTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeFeatureHeading",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeFeatureItemsJson",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeFeatureLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeFeatureSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeFeatureTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroBadge",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroDescription",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroHighlight",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroImageAlt",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroImageUrl",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroLine1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroLine3",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeHeroTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomePrimaryBtnLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomePrimaryBtnRoute",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeSecondaryBtnLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeSecondaryBtnRoute",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "publicPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NavAboutLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavBrandSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavBrandTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavClaimsLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavCommitteeLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavContactLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavDownloadsLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavEmailIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavEmailValue",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavHomeLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavLoginIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavLoginLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavLogoAlt",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavLogoUrl",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "NavMenuHead",
                table: "publicPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NavPhoneIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavPhoneValue",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NavRulesLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsBreadcrumbCurrentLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsBreadcrumbHomeLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsEmptyText",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsHeroSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsHeroTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsItemsJson",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsLoadingText",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsQuickLinksJson",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsSidebarQuoteText",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsSidebarQuoteTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficeAddress",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficeDay1Time",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutHeaderSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHeaderTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHistoryIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHistoryPara1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHistoryPara2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHistoryPara3",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHistoryPara4",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHistoryPara5",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutHistoryTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutMissionDescription",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutMissionIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutMissionTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutVisionDescription",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutVisionIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutVisionTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsHeroSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsHeroTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat1Icon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat1Label",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat1Value",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat2Icon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat2Label",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat2Value",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat3Icon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat3Label",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsStat3Value",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsTableHeadersJson",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ClaimsYearsRange",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "CommitteeHeaderSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "CommitteeHeaderTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "CommitteeMembersJson",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactEmailLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactFullNameLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactHeaderSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactHeaderTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactMessageLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactPhoneLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactSubjectLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactSubmitButtonLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "DownloadItemsJson",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "DownloadsHeaderSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "DownloadsHeaderTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterAddressLine1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterAddressLine2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterBrandDescription",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterBrandShortName",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterBrandSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterCopyrightText",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterEmailIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterEmailValue",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterLogoAlt",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterPhoneIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "FooterPhoneValue",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeAboutLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeAboutParagraph",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeAboutTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeFeatureHeading",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeFeatureItemsJson",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeFeatureLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeFeatureSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeFeatureTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroBadge",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroDescription",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroHighlight",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroImageAlt",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroImageUrl",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroLine1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroLine3",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeHeroTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomePrimaryBtnLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomePrimaryBtnRoute",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeSecondaryBtnLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "HomeSecondaryBtnRoute",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavAboutLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavBrandSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavBrandTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavClaimsLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavCommitteeLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavContactLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavDownloadsLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavEmailIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavEmailValue",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavHomeLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavLoginIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavLoginLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavLogoAlt",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavLogoUrl",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavMenuHead",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavPhoneIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavPhoneValue",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NavRulesLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsBreadcrumbCurrentLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsBreadcrumbHomeLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsEmptyText",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsHeroSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsHeroTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsItemsJson",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsLoadingText",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsQuickLinksJson",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsSidebarQuoteText",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsSidebarQuoteTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "OfficeAddress",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "OfficeDay1Time",
                table: "publicPages");

            migrationBuilder.RenameColumn(
                name: "RulesSectionsJson",
                table: "publicPages",
                newName: "NavbarTitle");

            migrationBuilder.RenameColumn(
                name: "RulesPreambleTitle",
                table: "publicPages",
                newName: "NavbarSubTitle");

            migrationBuilder.RenameColumn(
                name: "RulesPreamblePara5",
                table: "publicPages",
                newName: "NavbarPhone");

            migrationBuilder.RenameColumn(
                name: "RulesPreamblePara4",
                table: "publicPages",
                newName: "NavbarMenuJson");

            migrationBuilder.RenameColumn(
                name: "RulesPreamblePara3",
                table: "publicPages",
                newName: "NavbarMenuHead");

            migrationBuilder.RenameColumn(
                name: "RulesPreamblePara2",
                table: "publicPages",
                newName: "NavbarLogoAlt");

            migrationBuilder.RenameColumn(
                name: "RulesPreamblePara1",
                table: "publicPages",
                newName: "NavbarLoginLabel");

            migrationBuilder.RenameColumn(
                name: "RulesHeaderTitle",
                table: "publicPages",
                newName: "NavbarLoginIconClass");

            migrationBuilder.RenameColumn(
                name: "RulesHeaderSubTitle",
                table: "publicPages",
                newName: "NavbarEmail");

            migrationBuilder.RenameColumn(
                name: "PrivacyPara3",
                table: "publicPages",
                newName: "HeroTitleLine3");

            migrationBuilder.RenameColumn(
                name: "PrivacyPara2",
                table: "publicPages",
                newName: "HeroTitleLine1");

            migrationBuilder.RenameColumn(
                name: "PrivacyPara1",
                table: "publicPages",
                newName: "HeroTitleHighlight");

            migrationBuilder.RenameColumn(
                name: "PrivacyLine6",
                table: "publicPages",
                newName: "HeroSecondaryBtnRoute");

            migrationBuilder.RenameColumn(
                name: "PrivacyLine5",
                table: "publicPages",
                newName: "HeroSecondaryBtnLabel");

            migrationBuilder.RenameColumn(
                name: "PrivacyLine4",
                table: "publicPages",
                newName: "HeroPrimaryBtnRoute");

            migrationBuilder.RenameColumn(
                name: "PrivacyLine3",
                table: "publicPages",
                newName: "HeroPrimaryBtnLabel");

            migrationBuilder.RenameColumn(
                name: "PrivacyLine2",
                table: "publicPages",
                newName: "HeroDescription");

            migrationBuilder.RenameColumn(
                name: "PrivacyLine1",
                table: "publicPages",
                newName: "HeroBadge");

            migrationBuilder.RenameColumn(
                name: "PrivacyHeroTitle",
                table: "publicPages",
                newName: "FooterSubtitle");

            migrationBuilder.RenameColumn(
                name: "PrivacyHeroSubTitle",
                table: "publicPages",
                newName: "FooterShortName");

            migrationBuilder.RenameColumn(
                name: "PrivacyHeroBadge",
                table: "publicPages",
                newName: "FooterPhone");

            migrationBuilder.RenameColumn(
                name: "PrivacyHeading3",
                table: "publicPages",
                newName: "FooterEmail");

            migrationBuilder.RenameColumn(
                name: "PrivacyHeading2",
                table: "publicPages",
                newName: "FooterDescription");

            migrationBuilder.RenameColumn(
                name: "PrivacyHeading1",
                table: "publicPages",
                newName: "FooterBottomBarText");

            migrationBuilder.RenameColumn(
                name: "OfficeTitle",
                table: "publicPages",
                newName: "FooterAddress2");

            migrationBuilder.RenameColumn(
                name: "OfficePhone",
                table: "publicPages",
                newName: "FooterAddress1");

            migrationBuilder.RenameColumn(
                name: "OfficeHoursTitle",
                table: "publicPages",
                newName: "FeaturesItemsJson");

            migrationBuilder.RenameColumn(
                name: "OfficeEmail",
                table: "publicPages",
                newName: "FeaturesHeadingTitle");

            migrationBuilder.RenameColumn(
                name: "OfficeDay3Time",
                table: "publicPages",
                newName: "FeaturesHeadingSubtitle");

            migrationBuilder.RenameColumn(
                name: "OfficeDay2Time",
                table: "publicPages",
                newName: "FeaturesHeadingLabel");
        }
    }
}
