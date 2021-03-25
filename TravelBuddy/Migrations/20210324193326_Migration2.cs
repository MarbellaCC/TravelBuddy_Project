using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelBuddy.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3fb551a-aca2-46bb-8ac9-43aa2fe963f7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1b27bc1d-d240-4c85-b72d-a7a34b432e95", "9e0d226f-65ec-4210-801f-9e1ff10db8eb", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b27bc1d-d240-4c85-b72d-a7a34b432e95");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3fb551a-aca2-46bb-8ac9-43aa2fe963f7", "224ba689-2ca7-43cf-933e-eb8af6e6fe54", "Customer", "CUSTOMER" });
        }
    }
}
