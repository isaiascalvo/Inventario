using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ProductEntryCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "ProductEntries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "ProductEntries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "ProductEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "ProductEntries");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "ProductEntries");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "ProductEntries");
        }
    }
}
