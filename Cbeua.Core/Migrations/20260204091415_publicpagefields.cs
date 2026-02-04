using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class publicpagefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circles_States_StateId",
                table: "Circles");

            migrationBuilder.DropIndex(
                name: "IX_Circles_StateId",
                table: "Circles");

            migrationBuilder.AddColumn<string>(
                name: "FooterQuickHead",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagingCommitteeHeaderSubTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagingCommitteeHeaderTitle",
                table: "publicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterQuickHead",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ManagingCommitteeHeaderSubTitle",
                table: "publicPages");

            migrationBuilder.DropColumn(
                name: "ManagingCommitteeHeaderTitle",
                table: "publicPages");

            migrationBuilder.CreateIndex(
                name: "IX_Circles_StateId",
                table: "Circles",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Circles_States_StateId",
                table: "Circles",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId");
        }
    }
}
