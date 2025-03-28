using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.Migrations
{
    /// <inheritdoc />
    public partial class changedbranchdatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Pin",
                table: "Branchs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Branchs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Branchs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Phone", "Pin" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Branchs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Phone", "Pin" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Pin",
                table: "Branchs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "Branchs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Branchs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Phone", "Pin" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Branchs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Phone", "Pin" },
                values: new object[] { 0, 0 });
        }
    }
}
