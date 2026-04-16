using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class tt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTransactionUser");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StockTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_UserId",
                table: "StockTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Users_UserId",
                table: "StockTransactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Users_UserId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_UserId",
                table: "StockTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StockTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StockTransactionUser",
                columns: table => new
                {
                    StockTransactionsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactionUser", x => new { x.StockTransactionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_StockTransactionUser_StockTransactions_StockTransactionsId",
                        column: x => x.StockTransactionsId,
                        principalTable: "StockTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactionUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionUser_UsersId",
                table: "StockTransactionUser",
                column: "UsersId");
        }
    }
}
