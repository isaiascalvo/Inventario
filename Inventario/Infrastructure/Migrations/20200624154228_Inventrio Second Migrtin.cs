using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class InventrioSecondMigrtin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripton",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Vendors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "Descripton",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
