using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    public partial class newest_withuserbalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Balance", "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { 1000.0, new DateTime(2022, 12, 5, 11, 14, 34, 92, DateTimeKind.Utc).AddTicks(1190), new DateTime(2022, 12, 5, 11, 14, 34, 92, DateTimeKind.Utc).AddTicks(1193), "$2a$11$NH/hlOU.S/sXfLZFPps0M.1PcVsV6.XRMRO7MDu0QLXi6TsymhMHq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Balance", "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { 1000.0, new DateTime(2022, 12, 5, 11, 14, 34, 204, DateTimeKind.Utc).AddTicks(293), new DateTime(2022, 12, 5, 11, 14, 34, 204, DateTimeKind.Utc).AddTicks(299), "$2a$11$gbKfZUS2AqtrtS7AEfxQF.OFg.t2GmBe4mJNGVlBG4xp1Vb9GyxoO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Balance", "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { 1000.0, new DateTime(2022, 12, 5, 11, 14, 34, 315, DateTimeKind.Utc).AddTicks(8086), new DateTime(2022, 12, 5, 11, 14, 34, 315, DateTimeKind.Utc).AddTicks(8091), "$2a$11$uuQXyOh.4Eg4CDwYqT4EUuka8bP6O.1UH73dfeCM2wPVcbGtAw11." });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 1, 10, 18, 34, 556, DateTimeKind.Utc).AddTicks(9876), new DateTime(2022, 12, 1, 10, 18, 34, 556, DateTimeKind.Utc).AddTicks(9878), "$2a$11$GKITrmfmvtCJ/Ta8nNyGe.gjgk16xMHY068siraW.HGZaOTrGWdd." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 1, 10, 18, 34, 670, DateTimeKind.Utc).AddTicks(88), new DateTime(2022, 12, 1, 10, 18, 34, 670, DateTimeKind.Utc).AddTicks(94), "$2a$11$ZI.9RkjX5VshfvrJpsyJduqzAAwTB8uhxoRAtcliO.gaZAZ5mZX8W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 1, 10, 18, 34, 783, DateTimeKind.Utc).AddTicks(5258), new DateTime(2022, 12, 1, 10, 18, 34, 783, DateTimeKind.Utc).AddTicks(5264), "$2a$11$firrf1Ua4JJ5xE5qSSnXm.rcKrpCT/06EGtJxEvZI0sthe/2ISwjy" });
        }
    }
}
