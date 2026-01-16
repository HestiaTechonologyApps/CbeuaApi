using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPublicPageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutParagraph1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutParagraph2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutParagraph3",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutParagraph4",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutStatsJson",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadIcon",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadsCardIconClass",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadsCardTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadsContactButtonText",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsSectionHeadingLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsSectionHeadingTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsSectionQuickLinksHead",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewsTag",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RulesPreamblePara6",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutParagraph1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutParagraph2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutParagraph3",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutParagraph4",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "AboutStatsJson",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "DownloadIcon",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "DownloadsCardIconClass",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "DownloadsCardTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "DownloadsContactButtonText",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsSectionHeadingLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsSectionHeadingTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsSectionQuickLinksHead",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "NewsTag",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "RulesPreamblePara6",
                table: "publicPages");
        }
    }
}
