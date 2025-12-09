using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cbeua.Core.Migrations
{
    /// <inheritdoc />
    public partial class addedDayquote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayQuotes",
                columns: table => new
                {
                    DayQuoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    MonthCode = table.Column<int>(type: "int", nullable: false),
                    ToDayQuote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnformatedContent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayQuotes", x => x.DayQuoteId);
                });

            migrationBuilder.CreateTable(
                name: "MainPages",
                columns: table => new
                {
                    MainPageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorouselImage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorouselImage2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorouselImage3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoImage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoImage2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDesc1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDesc2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactLine3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phonenum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Faxnum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RulesRegulation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayQuote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPages", x => x.MainPageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayQuotes");

            migrationBuilder.DropTable(
                name: "MainPages");
        }
    }
}
