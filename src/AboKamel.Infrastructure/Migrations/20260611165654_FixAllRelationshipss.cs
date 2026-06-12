using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AboKamel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAllRelationshipss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerId1",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CustomerId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "Supports",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "Supports");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "Addresses",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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
        }
    }
}
