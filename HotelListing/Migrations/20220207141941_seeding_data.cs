using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Migrations
{
    public partial class seeding_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "eeb5d55c-e183-4fa0-bc0f-de69fe9e8a50", "3091865b-d727-4776-83d1-855846864d0c", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c4a130a2-28fa-4854-8e3c-da84f643945d", "c938cb3d-cd78-4927-800a-dd5f0b15917d", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4a130a2-28fa-4854-8e3c-da84f643945d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eeb5d55c-e183-4fa0-bc0f-de69fe9e8a50");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "58a1fd14-db06-4353-9668-dd39c38ef6c8", "3a157611-2d04-47d5-861e-0c6e70949c00", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4dd45d92-ae72-4883-afae-54135d94498d", "99a4c3c0-d1e8-4cf6-be0e-22f50e628ef8", "Administrator", "ADMINISTRATOR" });
        }
    }
}
