using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class ReceiptDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transatcions",
                table: "Transatcions");

            migrationBuilder.RenameTable(
                name: "Transatcions",
                newName: "Transactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Receipts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Receipts");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transatcions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transatcions",
                table: "Transatcions",
                column: "TransactionID");
        }
    }
}
