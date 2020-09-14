using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class creditanddebitcardschanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DebitCard_Discount",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DebitCard_Surcharge",
                table: "Payment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebitCard_Discount",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DebitCard_Surcharge",
                table: "Payment");
        }
    }
}
