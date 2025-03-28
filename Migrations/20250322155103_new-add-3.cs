using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.Migrations
{
    /// <inheritdoc />
    public partial class newadd3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 22, 21, 21, 3, 462, DateTimeKind.Local).AddTicks(8621));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 22, 21, 19, 30, 788, DateTimeKind.Local).AddTicks(6974));
        }
    }
}
