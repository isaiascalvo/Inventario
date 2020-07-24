using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class df : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Vendors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModificationBy",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Users",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModificationBy",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "StockMovements",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "StockMovements",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModificationBy",
                table: "StockMovements",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModificationBy",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Prices",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Prices",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModificationBy",
                table: "Prices",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Clients",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModificationBy",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Categories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModificationBy",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "Categories");
        }
    }
}
