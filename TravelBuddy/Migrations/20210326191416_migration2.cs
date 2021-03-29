using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelBuddy.Migrations
{
    public partial class migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6349b0e0-09db-4d32-be45-3862a3ac5e57");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "Travelers");

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    TravelerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interests_Travelers_TravelerId",
                        column: x => x.TravelerId,
                        principalTable: "Travelers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c96a9f7-f759-4651-b9cf-8d26b47afc4f", "05b2e16b-681f-45bc-8f1d-c32f98d8a1fa", "Customer", "CUSTOMER" });

            migrationBuilder.CreateIndex(
                name: "IX_Interests_TravelerId",
                table: "Interests",
                column: "TravelerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c96a9f7-f759-4651-b9cf-8d26b47afc4f");

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "Travelers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6349b0e0-09db-4d32-be45-3862a3ac5e57", "6cb26da6-91f5-47e1-b141-7db54f517348", "Customer", "CUSTOMER" });
        }
    }
}
