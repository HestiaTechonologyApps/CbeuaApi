using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class Addpublicpagefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NavMenuHead",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmailPlaceholder",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactFullNamePlaceholder",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactMessagePlaceholder",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ContactMessageRowNo",
                table: "publicPages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeAddress2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeAddress3",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeDay1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeDay2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeDay3",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeEmailIconClass",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeEmailLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficePhoneIconClass",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficePhoneLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeTitleIconClass",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactOfficeTitleLabel",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhoneNumberPlaceholder",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactSubjectPlaceholder",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactSubmitButtonIconClass",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading10",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading10Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading10Para2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading11",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading11Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading11Para2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading12",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading12Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading3Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading4",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading5",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading5Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading6",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading6Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading7",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading7Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading8",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading8Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading8Para2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading8Para3",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading8Para4",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9Para1",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9Para2",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9Para3",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9Para4",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9Para5",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9Para6",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyHeading9Para7",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacyLine7",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacySubHeading4",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacySubHeading8",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrivacySubHeading9",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmailPlaceholder",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactFullNamePlaceholder",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactMessagePlaceholder",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactMessageRowNo",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeAddress2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeAddress3",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeDay1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeDay2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeDay3",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeEmailIconClass",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeEmailLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficePhoneIconClass",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficePhoneLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeTitleIconClass",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactOfficeTitleLabel",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactPhoneNumberPlaceholder",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactSubjectPlaceholder",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ContactSubmitButtonIconClass",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading10",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading10Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading10Para2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading11",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading11Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading11Para2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading12",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading12Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading3Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading4",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading5",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading5Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading6",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading6Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading7",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading7Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading8",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading8Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading8Para2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading8Para3",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading8Para4",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9Para1",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9Para2",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9Para3",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9Para4",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9Para5",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9Para6",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyHeading9Para7",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacyLine7",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacySubHeading4",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacySubHeading8",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "PrivacySubHeading9",
                table: "publicPages");

            migrationBuilder.AlterColumn<bool>(
                name: "NavMenuHead",
                table: "publicPages",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
