using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Cost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "TransactionItemModifiers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "TransactionEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "TransactionItemModifiers");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "TransactionEntries");
        }
    }
}
