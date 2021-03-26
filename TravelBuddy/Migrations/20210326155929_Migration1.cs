using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelBuddy.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67601f55-e1a6-4149-8e22-f6e202168475");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Travelers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Travelers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6349b0e0-09db-4d32-be45-3862a3ac5e57", "6cb26da6-91f5-47e1-b141-7db54f517348", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6349b0e0-09db-4d32-be45-3862a3ac5e57");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Travelers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Travelers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "67601f55-e1a6-4149-8e22-f6e202168475", "fac4c9a5-7c0f-495a-8975-96457b1e850c", "Customer", "CUSTOMER" });
        }
    }
}
