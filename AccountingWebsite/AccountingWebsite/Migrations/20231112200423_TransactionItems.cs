using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations
{
    /// <inheritdoc />
    public partial class TransactionItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ShortName = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    InnerMeasurementID = table.Column<int>(type: "INTEGER", nullable: true),
                    IsCase = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsContainer = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Measurements_Measurements_InnerMeasurementID",
                        column: x => x.InnerMeasurementID,
                        principalTable: "Measurements",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TransactionEntries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionID = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    MeasurementID = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEntries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransactionEntries_Measurements_MeasurementID",
                        column: x => x.MeasurementID,
                        principalTable: "Measurements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionEntries_TransactionItems_TransactionItemID",
                        column: x => x.TransactionItemID,
                        principalTable: "TransactionItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionEntries_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID");
                });

            migrationBuilder.CreateTable(
                name: "TransactionItemMappings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VendorName = table.Column<string>(type: "TEXT", nullable: true),
                    Keyphrases = table.Column<string>(type: "TEXT", nullable: true),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItemMappings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransactionItemMappings_TransactionItems_ItemID",
                        column: x => x.ItemID,
                        principalTable: "TransactionItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionItemModifiers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionEntryID = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifierID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItemModifiers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TransactionItemModifiers_TransactionEntries_TransactionEntryID",
                        column: x => x.TransactionEntryID,
                        principalTable: "TransactionEntries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionItemModifiers_TransactionItems_ModifierID",
                        column: x => x.ModifierID,
                        principalTable: "TransactionItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_InnerMeasurementID",
                table: "Measurements",
                column: "InnerMeasurementID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntries_MeasurementID",
                table: "TransactionEntries",
                column: "MeasurementID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntries_TransactionID",
                table: "TransactionEntries",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntries_TransactionItemID",
                table: "TransactionEntries",
                column: "TransactionItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItemMappings_ItemID",
                table: "TransactionItemMappings",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItemModifiers_ModifierID",
                table: "TransactionItemModifiers",
                column: "ModifierID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItemModifiers_TransactionEntryID",
                table: "TransactionItemModifiers",
                column: "TransactionEntryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionItemMappings");

            migrationBuilder.DropTable(
                name: "TransactionItemModifiers");

            migrationBuilder.DropTable(
                name: "TransactionEntries");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "TransactionItems");
        }
    }
}
