using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackService.Migrations
{
    public partial class firstmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    DeliveryDriverId = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "DeliveryDriverId", "OrderId", "Rating", "RestaurantId", "ReviewDate", "ReviewText", "UserId" },
                values: new object[] { 1, 3, 2, 5, 2, new DateTime(2022, 12, 5, 10, 28, 47, 891, DateTimeKind.Local).AddTicks(7911), "Maden var god og blev leveret hurtigt", 1 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "DeliveryDriverId", "OrderId", "Rating", "RestaurantId", "ReviewDate", "ReviewText", "UserId" },
                values: new object[] { 2, 3, 1, 1, 2, new DateTime(2022, 12, 5, 10, 28, 47, 891, DateTimeKind.Local).AddTicks(7921), "Maden var dårlig og blev leveret efter 3 timer", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
