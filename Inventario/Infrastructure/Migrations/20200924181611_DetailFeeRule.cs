using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class DetailFeeRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeeRuleId",
                table: "Details",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Details_FeeRuleId",
                table: "Details",
                column: "FeeRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_FeeRules_FeeRuleId",
                table: "Details",
                column: "FeeRuleId",
                principalTable: "FeeRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_FeeRules_FeeRuleId",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_FeeRuleId",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "FeeRuleId",
                table: "Details");
        }
    }
}
