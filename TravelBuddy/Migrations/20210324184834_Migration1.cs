using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelBuddy.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Activity_ActivityId",
                table: "Days");

            migrationBuilder.DropForeignKey(
                name: "FK_Travelers_Days_DayId",
                table: "Travelers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bff96b4b-b034-4651-9f21-ae37c0715c2b");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Travelers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Days",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3fb551a-aca2-46bb-8ac9-43aa2fe963f7", "224ba689-2ca7-43cf-933e-eb8af6e6fe54", "Customer", "CUSTOMER" });

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Activities_ActivityId",
                table: "Days",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Travelers_Days_DayId",
                table: "Travelers",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Activities_ActivityId",
                table: "Days");

            migrationBuilder.DropForeignKey(
                name: "FK_Travelers_Days_DayId",
                table: "Travelers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3fb551a-aca2-46bb-8ac9-43aa2fe963f7");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Travelers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Days",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bff96b4b-b034-4651-9f21-ae37c0715c2b", "ae406cde-c203-49fc-a13e-076946613d7d", "Customer", "CUSTOMER" });

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Activity_ActivityId",
                table: "Days",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Travelers_Days_DayId",
                table: "Travelers",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
