using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    public partial class Testdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CardType", "CreatedAt", "PaymentType" },
                values: new object[] { "Dankort", new DateTime(2022, 12, 7, 16, 22, 28, 561, DateTimeKind.Local).AddTicks(5542), "CreditCard" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CardType", "CreatedAt", "PaymentType" },
                values: new object[] { null, new DateTime(2022, 12, 7, 16, 21, 36, 641, DateTimeKind.Local).AddTicks(2612), "0" });
        }
    }
}
