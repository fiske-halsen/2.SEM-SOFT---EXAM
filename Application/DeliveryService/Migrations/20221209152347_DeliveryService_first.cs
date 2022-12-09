using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Migrations
{
    public partial class DeliveryService_first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryPersonId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    isCancelled = table.Column<bool>(type: "bit", nullable: false),
                    TimeToDelivery = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryId);
                });

            migrationBuilder.InsertData(
                table: "Deliveries",
                columns: new[] { "DeliveryId", "CreatedDate", "DeliveryPersonId", "IsDelivered", "OrderId", "RestaurantId", "TimeToDelivery", "UserEmail", "isCancelled" },
                values: new object[] { 1, new DateTime(2022, 12, 9, 15, 23, 47, 713, DateTimeKind.Utc).AddTicks(2082), 3, false, 1, 1, new DateTime(2022, 12, 9, 15, 53, 47, 713, DateTimeKind.Utc).AddTicks(2083), "phillip.andersen1999@gmail.com", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliveries");
        }
    }
}
