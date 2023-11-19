using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingModifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeasurementID",
                table: "TransactionItemModifiers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TransactionItemModifiers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentModifierID",
                table: "TransactionItemModifiers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "TransactionItemModifiers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItemModifiers_MeasurementID",
                table: "TransactionItemModifiers",
                column: "MeasurementID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItemModifiers_ParentModifierID",
                table: "TransactionItemModifiers",
                column: "ParentModifierID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItemModifiers_Measurements_MeasurementID",
                table: "TransactionItemModifiers",
                column: "MeasurementID",
                principalTable: "Measurements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItemModifiers_TransactionItemModifiers_ParentModifierID",
                table: "TransactionItemModifiers",
                column: "ParentModifierID",
                principalTable: "TransactionItemModifiers",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItemModifiers_Measurements_MeasurementID",
                table: "TransactionItemModifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItemModifiers_TransactionItemModifiers_ParentModifierID",
                table: "TransactionItemModifiers");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItemModifiers_MeasurementID",
                table: "TransactionItemModifiers");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItemModifiers_ParentModifierID",
                table: "TransactionItemModifiers");

            migrationBuilder.DropColumn(
                name: "MeasurementID",
                table: "TransactionItemModifiers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TransactionItemModifiers");

            migrationBuilder.DropColumn(
                name: "ParentModifierID",
                table: "TransactionItemModifiers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TransactionItemModifiers");
        }
    }
}
