using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AboKamel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ResendOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastResendDate",
                table: "TwoFactorVerifies",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResendAttempts",
                table: "TwoFactorVerifies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastResendDate",
                table: "TwoFactorVerifies");

            migrationBuilder.DropColumn(
                name: "ResendAttempts",
                table: "TwoFactorVerifies");
        }
    }
}
