using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedcompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MainPages_CompanyId",
                table: "MainPages",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainPages_Companies_CompanyId",
                table: "MainPages",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainPages_Companies_CompanyId",
                table: "MainPages");

            migrationBuilder.DropIndex(
                name: "IX_MainPages_CompanyId",
                table: "MainPages");
        }
    }
}
