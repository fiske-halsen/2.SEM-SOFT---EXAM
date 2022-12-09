using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackService.Migrations
{
    public partial class evenNewerFeeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "DeliveryDriverId", "OrderId", "Rating", "RestaurantId", "ReviewDate", "ReviewText", "UserId" },
                values: new object[] { 1, 2, 2, 5, 1, new DateTime(2022, 12, 9, 21, 41, 44, 261, DateTimeKind.Local).AddTicks(3421), "Maden var god og blev leveret hurtigt", 1 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "DeliveryDriverId", "OrderId", "Rating", "RestaurantId", "ReviewDate", "ReviewText", "UserId" },
                values: new object[] { 2, 2, 1, 1, 1, new DateTime(2022, 12, 9, 21, 41, 44, 261, DateTimeKind.Local).AddTicks(3463), "Maden var dårlig og blev leveret efter 3 timer", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
