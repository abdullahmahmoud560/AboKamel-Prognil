using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AboKamel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAllRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "Addresses",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId1",
                table: "Addresses",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_CustomerId1",
                table: "Addresses",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerId1",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CustomerId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Addresses");
        }
    }
}
