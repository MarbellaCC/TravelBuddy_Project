using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelBuddy.Data.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd2d89d4-656f-423c-994a-d90e318b2da0");

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantMaxDistance = table.Column<double>(nullable: true),
                    AdventureMaxDistance = table.Column<double>(nullable: true),
                    TypeOfRestaurant = table.Column<string>(nullable: true),
                    TypeOfAdventure = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Travelers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    DestinationCity = table.Column<string>(nullable: true),
                    DestinationState = table.Column<string>(nullable: true),
                    DestinationCountry = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Lodging = table.Column<string>(nullable: true),
                    IdentityUserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travelers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Travelers_AspNetUsers_IdentityUserID",
                        column: x => x.IdentityUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "07e23804-94b1-46b1-a700-44d5d6eb7c3c", "0dd8576d-c6a4-4933-a045-c6d51e03bc0b", "Customer", "CUSTOMER" });

            migrationBuilder.CreateIndex(
                name: "IX_Travelers_IdentityUserID",
                table: "Travelers",
                column: "IdentityUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "Travelers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07e23804-94b1-46b1-a700-44d5d6eb7c3c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dd2d89d4-656f-423c-994a-d90e318b2da0", "8a597861-63e1-4a22-97bf-37f5fa388431", "Customer", "CUSTOMER" });
        }
    }
}
