using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class softDeleteError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEmailSettings_Users_UserId1",
                table: "UserEmailSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserEmailSettings_UserId1",
                table: "UserEmailSettings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserEmailSettings");

          

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

         
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserEmailSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_UserEmailSettings_UserId1",
                table: "UserEmailSettings",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEmailSettings_Users_UserId1",
                table: "UserEmailSettings",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
