using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "publicPages",
                columns: table => new
                {
                    PublicPageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NavbarTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarSubTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarLogoAlt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarMenuHead = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarMenuJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarLoginLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarLoginIconClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavbarEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroBadge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroTitleLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroTitleHighlight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroTitleLine3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroPrimaryBtnLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroPrimaryBtnRoute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroSecondaryBtnLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroSecondaryBtnRoute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturesHeadingLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturesHeadingTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturesHeadingSubtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturesItemsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterSubtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterQuickLinksJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterOfficeHoursJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterBottomBarText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicPages", x => x.PublicPageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "publicPages");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }
    }
}
