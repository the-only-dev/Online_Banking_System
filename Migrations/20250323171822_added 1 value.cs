using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.Migrations
{
    /// <inheritdoc />
    public partial class added1value : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountCount", "Address", "BranchId", "BusinessName", "BusinessType", "CustomerType", "Email", "FullName", "Job", "Password", "Phone", "Pin", "TaxId", "Username" },
                values: new object[] { 1, null, "Admin", 1, "Admin", "Admin", "Admin", "Admin@gmail.com", "Admin", "Admin", "Admin", "0000000000", "Admin", "Admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountCount", "Address", "BranchId", "BusinessName", "BusinessType", "CustomerType", "Email", "FullName", "Job", "Password", "Phone", "Pin", "TaxId", "Username" },
                values: new object[] { 8, null, "not available", null, "Some", "Steel", "Business", "Something@gmail.com", "Something New", "CEO", null, null, "321422", "GSTIN859340", null });
        }
    }
}
