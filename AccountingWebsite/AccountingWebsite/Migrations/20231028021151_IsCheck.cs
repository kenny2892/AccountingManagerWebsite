using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class IsCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transatcions");

            migrationBuilder.AddColumn<bool>(
                name: "IsCheck",
                table: "Transatcions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCheck",
                table: "Transatcions");

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "Transatcions",
                type: "TEXT",
                nullable: true);
        }
    }
}
