using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class CuilNulleable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_CUIL",
                table: "Vendors");

            migrationBuilder.AlterColumn<string>(
                name: "CUIL",
                table: "Vendors",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CUIL",
                table: "Vendors",
                column: "CUIL",
                unique: true,
                filter: "[CUIL] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_CUIL",
                table: "Vendors");

            migrationBuilder.AlterColumn<string>(
                name: "CUIL",
                table: "Vendors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CUIL",
                table: "Vendors",
                column: "CUIL",
                unique: true);
        }
    }
}
