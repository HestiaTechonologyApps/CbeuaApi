using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addeduserRoleRight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRoleRights",
                columns: table => new
                {
                    UserRoleRightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerName = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleRights", x => x.UserRoleRightId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoleRights");
        }
    }
}
