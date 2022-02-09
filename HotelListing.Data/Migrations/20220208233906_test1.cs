using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Data.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28afbb34-d001-471d-a85a-9b3369c0d6d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36263031-e40d-4ae3-b336-4cad4fc56a06");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8f175ff1-83a1-45a0-8f86-e7a7b53b795a", "628f4830-b1bd-45ff-8db9-f1691d9feec5", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4795aaf2-813b-4752-8be2-9745d7fe28b5", "580ae051-d459-4114-8b1a-d28d0d83ac74", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4795aaf2-813b-4752-8be2-9745d7fe28b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f175ff1-83a1-45a0-8f86-e7a7b53b795a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28afbb34-d001-471d-a85a-9b3369c0d6d3", "98f158a8-94fd-4334-9e44-7420c66dc1db", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "36263031-e40d-4ae3-b336-4cad4fc56a06", "382014c5-6468-46f3-9bc7-9c904ea385c2", "Administrator", "ADMINISTRATOR" });
        }
    }
}
