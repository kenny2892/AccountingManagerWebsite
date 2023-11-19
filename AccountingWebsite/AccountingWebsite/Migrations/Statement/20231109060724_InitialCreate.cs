using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingWebsite.Migrations.Statement
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatementMappings",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bank = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionIdHeader = table.Column<string>(type: "TEXT", nullable: true),
                    PurchasingDateHeader = table.Column<string>(type: "TEXT", nullable: true),
                    PostingDateHeader = table.Column<string>(type: "TEXT", nullable: true),
                    AmountHeader = table.Column<string>(type: "TEXT", nullable: true),
                    CheckNumberHeader = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceNumberHeader = table.Column<string>(type: "TEXT", nullable: true),
                    DescriptionHeader = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryOneHeader = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryTwoHeader = table.Column<string>(type: "TEXT", nullable: true),
                    TypeHeader = table.Column<string>(type: "TEXT", nullable: true),
                    BalanceHeader = table.Column<string>(type: "TEXT", nullable: true),
                    MemoHeader = table.Column<string>(type: "TEXT", nullable: true),
                    ExtendedDescriptionHeader = table.Column<string>(type: "TEXT", nullable: true),
                    VendorNameHeader = table.Column<string>(type: "TEXT", nullable: true),
                    IsPurchaseHeader = table.Column<string>(type: "TEXT", nullable: true),
                    IsCreditHeader = table.Column<string>(type: "TEXT", nullable: true),
                    IsCheckHeader = table.Column<string>(type: "TEXT", nullable: true),
                    IsCreditPaymentHeader = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementMappings", x => x.Index);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatementMappings");
        }
    }
}
