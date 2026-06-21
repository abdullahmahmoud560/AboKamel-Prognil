using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AboKamel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEstablishmentTypeToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstablishmentType",
                table: "Customers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstablishmentType",
                table: "Customers");
        }
    }
}
