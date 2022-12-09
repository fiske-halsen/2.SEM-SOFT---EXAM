using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantService.Migrations
{
    public partial class errortest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExceptionDtos",
                columns: new[] { "Id", "Message", "StatusCode" },
                values: new object[] { 1, "error msg", 400 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExceptionDtos",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
