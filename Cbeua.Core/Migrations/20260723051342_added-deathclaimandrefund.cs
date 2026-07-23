using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addeddeathclaimandrefund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "RefundContributions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "RefundContributions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "RefundContributions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "DeathClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "DeathClaims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "DeathClaims",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "RefundContributions");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "RefundContributions");

            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "RefundContributions");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "DeathClaims");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "DeathClaims");

            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "DeathClaims");
        }
    }
}
