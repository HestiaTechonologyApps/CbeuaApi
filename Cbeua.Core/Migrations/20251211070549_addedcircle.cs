using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedcircle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circles_States_StateId",
                table: "Circles");

            migrationBuilder.DropIndex(
                name: "IX_Circles_StateId",
                table: "Circles");
        }
    }
}
