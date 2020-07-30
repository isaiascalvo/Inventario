using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class corrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEntryLines_ProductEntries_ProductId",
                table: "ProductEntryLines");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntryLines_ProductEntryId",
                table: "ProductEntryLines",
                column: "ProductEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEntryLines_ProductEntries_ProductEntryId",
                table: "ProductEntryLines",
                column: "ProductEntryId",
                principalTable: "ProductEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEntryLines_ProductEntries_ProductEntryId",
                table: "ProductEntryLines");

            migrationBuilder.DropIndex(
                name: "IX_ProductEntryLines_ProductEntryId",
                table: "ProductEntryLines");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEntryLines_ProductEntries_ProductId",
                table: "ProductEntryLines",
                column: "ProductId",
                principalTable: "ProductEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
