using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transatcions",
                columns: table => new
                {
                    TransactionID = table.Column<string>(type: "TEXT", nullable: false),
                    Bank = table.Column<string>(type: "TEXT", nullable: true),
                    PurchasingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TransactionType = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    CheckNumber = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryOne = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryTwo = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Balance = table.Column<double>(type: "REAL", nullable: false),
                    Memo = table.Column<string>(type: "TEXT", nullable: true),
                    ExtendedDescription = table.Column<string>(type: "TEXT", nullable: true),
                    VendorName = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiptRelativeFilePath = table.Column<string>(type: "TEXT", nullable: true),
                    IsPurchase = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transatcions", x => x.TransactionID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transatcions");
        }
    }
}
