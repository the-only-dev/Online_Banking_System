using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.Migrations
{
    /// <inheritdoc />
    public partial class addedemptycolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCount",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "OldPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "OldPassword",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPassword",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AccountCount",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "AccountCount",
                value: null);
        }
    }
}
