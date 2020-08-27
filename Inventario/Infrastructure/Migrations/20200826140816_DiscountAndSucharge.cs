using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class DiscountAndSucharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CreditCard_Discount",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Surcharge",
                table: "Payment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CreditCard_Discount",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Surcharge",
                table: "Payment");
        }
    }
}
