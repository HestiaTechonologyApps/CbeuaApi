using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class renamedHRMS_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMEmployees",
                table: "HRMEmployees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HrmEmployeeAwards",
                table: "HrmEmployeeAwards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMDocumentTypes",
                table: "HRMDocumentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMDesignations",
                table: "HRMDesignations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMDepartments",
                table: "HRMDepartments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMAwardTypes",
                table: "HRMAwardTypes");

            migrationBuilder.RenameTable(
                name: "HRMEmployees",
                newName: "HRMS_Employee");

            migrationBuilder.RenameTable(
                name: "HrmEmployeeAwards",
                newName: "HRMS_EmployeeAward");

            migrationBuilder.RenameTable(
                name: "HRMDocumentTypes",
                newName: "HRMS_DocumentType");

            migrationBuilder.RenameTable(
                name: "HRMDesignations",
                newName: "HRMS_Designation");

            migrationBuilder.RenameTable(
                name: "HRMDepartments",
                newName: "HRMS_Department");

            migrationBuilder.RenameTable(
                name: "HRMAwardTypes",
                newName: "HRMS_AwardType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMS_Employee",
                table: "HRMS_Employee",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMS_EmployeeAward",
                table: "HRMS_EmployeeAward",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMS_DocumentType",
                table: "HRMS_DocumentType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMS_Designation",
                table: "HRMS_Designation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMS_Department",
                table: "HRMS_Department",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMS_AwardType",
                table: "HRMS_AwardType",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMS_EmployeeAward",
                table: "HRMS_EmployeeAward");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMS_Employee",
                table: "HRMS_Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMS_DocumentType",
                table: "HRMS_DocumentType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMS_Designation",
                table: "HRMS_Designation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMS_Department",
                table: "HRMS_Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRMS_AwardType",
                table: "HRMS_AwardType");

            migrationBuilder.RenameTable(
                name: "HRMS_EmployeeAward",
                newName: "HrmEmployeeAwards");

            migrationBuilder.RenameTable(
                name: "HRMS_Employee",
                newName: "HRMEmployees");

            migrationBuilder.RenameTable(
                name: "HRMS_DocumentType",
                newName: "HRMDocumentTypes");

            migrationBuilder.RenameTable(
                name: "HRMS_Designation",
                newName: "HRMDesignations");

            migrationBuilder.RenameTable(
                name: "HRMS_Department",
                newName: "HRMDepartments");

            migrationBuilder.RenameTable(
                name: "HRMS_AwardType",
                newName: "HRMAwardTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HrmEmployeeAwards",
                table: "HrmEmployeeAwards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMEmployees",
                table: "HRMEmployees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMDocumentTypes",
                table: "HRMDocumentTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMDesignations",
                table: "HRMDesignations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMDepartments",
                table: "HRMDepartments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRMAwardTypes",
                table: "HRMAwardTypes",
                column: "Id");
        }
    }
}
