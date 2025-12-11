using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedbranchcirckerelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Branches_CircleId",
                table: "Branches",
                column: "CircleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Circles_CircleId",
                table: "Branches",
                column: "CircleId",
                principalTable: "Circles",
                principalColumn: "CircleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Circles_CircleId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CircleId",
                table: "Branches");
        }
    }
}
