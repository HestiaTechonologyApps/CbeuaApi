using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class renamedHRMS_Branch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMBranches",
                table: "HRMBranches");

            migrationBuilder.RenameTable(
                name: "HRMBranches",
                newName: "HRMS_Branch");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMS_Branch",
                table: "HRMS_Branch",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMS_Branch",
                table: "HRMS_Branch");

            migrationBuilder.RenameTable(
                name: "HRMS_Branch",
                newName: "HRMBranches");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMBranches",
                table: "HRMBranches",
                column: "Id");
        }
    }
}
