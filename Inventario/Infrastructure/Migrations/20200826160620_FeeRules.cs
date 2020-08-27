using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class FeeRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Payment_PaymentId1",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_PaymentId1",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PaymentId1",
                table: "Sales");

            migrationBuilder.CreateTable(
                name: "FeeRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastModificationBy = table.Column<Guid>(nullable: true),
                    LastModificationAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FeesAmountTo = table.Column<int>(nullable: false),
                    Percentage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeRules_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PaymentId",
                table: "Sales",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeRules_ProductId",
                table: "FeeRules",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Payment_PaymentId",
                table: "Sales",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Payment_PaymentId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "FeeRules");

            migrationBuilder.DropIndex(
                name: "IX_Sales_PaymentId",
                table: "Sales");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId1",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PaymentId1",
                table: "Sales",
                column: "PaymentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Payment_PaymentId1",
                table: "Sales",
                column: "PaymentId1",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
