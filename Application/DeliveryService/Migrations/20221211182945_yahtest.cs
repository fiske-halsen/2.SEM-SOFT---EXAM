using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Migrations
{
    public partial class yahtest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCancelled",
                table: "Deliveries");

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "TimeToDelivery" },
                values: new object[] { new DateTime(2022, 12, 11, 18, 29, 44, 880, DateTimeKind.Utc).AddTicks(1150), new DateTime(2022, 12, 11, 18, 59, 44, 880, DateTimeKind.Utc).AddTicks(1151) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCancelled",
                table: "Deliveries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "TimeToDelivery" },
                values: new object[] { new DateTime(2022, 12, 9, 15, 23, 47, 713, DateTimeKind.Utc).AddTicks(2082), new DateTime(2022, 12, 9, 15, 53, 47, 713, DateTimeKind.Utc).AddTicks(2083) });
        }
    }
}
