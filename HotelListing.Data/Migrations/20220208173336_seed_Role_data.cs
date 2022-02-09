using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Data.Migrations
{
    public partial class seed_Role_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "753f41a6-8148-4413-a7eb-3cfa15287f5a", "e78e416b-0ec2-4aad-965c-3c7a46597cb7", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ee13db13-875d-4bfa-af9e-62cb66676970", "eb4f5178-03f2-4f8f-8894-369a811a652e", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "753f41a6-8148-4413-a7eb-3cfa15287f5a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee13db13-875d-4bfa-af9e-62cb66676970");
        }
    }
}
