using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class NullableTransactionForModifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItemModifiers_TransactionEntries_TransactionEntryID",
                table: "TransactionItemModifiers");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionEntryID",
                table: "TransactionItemModifiers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItemModifiers_TransactionEntries_TransactionEntryID",
                table: "TransactionItemModifiers",
                column: "TransactionEntryID",
                principalTable: "TransactionEntries",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItemModifiers_TransactionEntries_TransactionEntryID",
                table: "TransactionItemModifiers");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionEntryID",
                table: "TransactionItemModifiers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItemModifiers_TransactionEntries_TransactionEntryID",
                table: "TransactionItemModifiers",
                column: "TransactionEntryID",
                principalTable: "TransactionEntries",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
