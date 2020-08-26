using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class prices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Prices");

            migrationBuilder.AddColumn<int>(
                name: "PriceType",
                table: "Prices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceType",
                table: "Prices");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
