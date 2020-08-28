using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Payment_FeeRuleId",
                table: "Payment",
                column: "FeeRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_FeeRules_FeeRuleId",
                table: "Payment",
                column: "FeeRuleId",
                principalTable: "FeeRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_FeeRules_FeeRuleId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_FeeRuleId",
                table: "Payment");
        }
    }
}
