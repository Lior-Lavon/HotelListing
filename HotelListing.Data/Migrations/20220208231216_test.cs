using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "753f41a6-8148-4413-a7eb-3cfa15287f5a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee13db13-875d-4bfa-af9e-62cb66676970");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28afbb34-d001-471d-a85a-9b3369c0d6d3", "98f158a8-94fd-4334-9e44-7420c66dc1db", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "36263031-e40d-4ae3-b336-4cad4fc56a06", "382014c5-6468-46f3-9bc7-9c904ea385c2", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "753f41a6-8148-4413-a7eb-3cfa15287f5a", "e78e416b-0ec2-4aad-965c-3c7a46597cb7", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ee13db13-875d-4bfa-af9e-62cb66676970", "eb4f5178-03f2-4f8f-8894-369a811a652e", "Administrator", "ADMINISTRATOR" });
        }
    }
}
