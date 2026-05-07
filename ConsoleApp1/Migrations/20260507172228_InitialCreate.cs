using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TriangleRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Side1 = table.Column<float>(type: "real", nullable: false),
                    Side2 = table.Column<float>(type: "real", nullable: false),
                    Side3 = table.Column<float>(type: "real", nullable: false),
                    TriangleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriangleRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TriangleRecords_Sides",
                table: "TriangleRecords",
                columns: new[] { "Side1", "Side2", "Side3" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TriangleRecords");
        }
    }
}
