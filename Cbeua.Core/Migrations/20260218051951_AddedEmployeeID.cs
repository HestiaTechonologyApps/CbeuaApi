using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmployeeID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesignationId",
                table: "HRMS_Employee",
                newName: "HRMDesignationId");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "HRMS_Employee",
                newName: "HRMDepartmentId");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "HRMS_Employee",
                newName: "HRMBranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HRMDesignationId",
                table: "HRMS_Employee",
                newName: "DesignationId");

            migrationBuilder.RenameColumn(
                name: "HRMDepartmentId",
                table: "HRMS_Employee",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "HRMBranchId",
                table: "HRMS_Employee",
                newName: "BranchId");
        }
    }
}
