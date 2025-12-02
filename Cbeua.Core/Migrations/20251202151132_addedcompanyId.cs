using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedcompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ManagingComitees",
                newName: "ManagingComitteeName");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ManagingComitees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ManagingComitees");

            migrationBuilder.RenameColumn(
                name: "ManagingComitteeName",
                table: "ManagingComitees",
                newName: "Name");
        }
    }
}
