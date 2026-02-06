using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedreporttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "ReportTypeId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReportTypes",
                columns: table => new
                {
                    ReportTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTypes", x => x.ReportTypeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportTypes");

            migrationBuilder.DropColumn(
                name: "ReportTypeId",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "ReportType",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
