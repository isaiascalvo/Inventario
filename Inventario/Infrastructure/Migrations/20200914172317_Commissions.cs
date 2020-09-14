using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Commissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Fees");

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "ProductEntries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "MiscellaneousExpenses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Commissions",
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
                    VendorId = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    PaymentType = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commissions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntries_VendorId",
                table: "ProductEntries",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_ClientId",
                table: "Commissions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_VendorId",
                table: "Commissions",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEntries_Vendors_VendorId",
                table: "ProductEntries",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEntries_Vendors_VendorId",
                table: "ProductEntries");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropIndex(
                name: "IX_ProductEntries_VendorId",
                table: "ProductEntries");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "ProductEntries");

            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "MiscellaneousExpenses");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Fees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
