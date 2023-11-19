using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations.Vendor
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionKeyphrases = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiptKeyphrases = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiptTotalKeyphrases = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryOne = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryTwo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Name);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
