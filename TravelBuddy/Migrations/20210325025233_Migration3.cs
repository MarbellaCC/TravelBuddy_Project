using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelBuddy.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b27bc1d-d240-4c85-b72d-a7a34b432e95");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0db1d588-1a67-48db-beee-8436dacff287", "45601da3-78aa-4363-8e3e-daa23eefaf52", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0db1d588-1a67-48db-beee-8436dacff287");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1b27bc1d-d240-4c85-b72d-a7a34b432e95", "9e0d226f-65ec-4210-801f-9e1ff10db8eb", "Customer", "CUSTOMER" });
        }
    }
}
