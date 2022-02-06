using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Migrations
{
    public partial class update_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7246a83-0577-4a1b-a4d6-ebadb5c4bf68");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd420dab-9a23-44af-b5ad-7bae8d5028c7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "58a1fd14-db06-4353-9668-dd39c38ef6c8", "3a157611-2d04-47d5-861e-0c6e70949c00", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4dd45d92-ae72-4883-afae-54135d94498d", "99a4c3c0-d1e8-4cf6-be0e-22f50e628ef8", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dd45d92-ae72-4883-afae-54135d94498d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58a1fd14-db06-4353-9668-dd39c38ef6c8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cd420dab-9a23-44af-b5ad-7bae8d5028c7", "730b45f4-5608-44d5-8ac4-a5cc95e1e001", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b7246a83-0577-4a1b-a4d6-ebadb5c4bf68", "1650d2bc-ab65-482c-8bde-c1a0d34277b4", "Administrator", "ADMINISTRATOR" });
        }
    }
}
