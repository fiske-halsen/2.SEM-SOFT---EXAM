using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    public partial class newest_Updated_properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 5, 11, 16, 8, 464, DateTimeKind.Utc).AddTicks(1904), new DateTime(2022, 12, 5, 11, 16, 8, 464, DateTimeKind.Utc).AddTicks(1906), "$2a$11$ICbPMppv0XMpM/5GmI9/qOsLg9x8bJtfScUPs7G3/4LBVnzOs9NcG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 5, 11, 16, 8, 574, DateTimeKind.Utc).AddTicks(9385), new DateTime(2022, 12, 5, 11, 16, 8, 574, DateTimeKind.Utc).AddTicks(9392), "$2a$11$PIe18FWerkm0YH9dhRX5vu4mWDD3MCHnszyhISNhVO.H38eHV4OwG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 5, 11, 16, 8, 686, DateTimeKind.Utc).AddTicks(1393), new DateTime(2022, 12, 5, 11, 16, 8, 686, DateTimeKind.Utc).AddTicks(1400), "$2a$11$8UpvpU.6tPDbeaO8WkNmEeh0JAzPetrJXmpe9ZOzzfa/Hfg7L9rPW" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 5, 11, 14, 34, 92, DateTimeKind.Utc).AddTicks(1190), new DateTime(2022, 12, 5, 11, 14, 34, 92, DateTimeKind.Utc).AddTicks(1193), "$2a$11$NH/hlOU.S/sXfLZFPps0M.1PcVsV6.XRMRO7MDu0QLXi6TsymhMHq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 5, 11, 14, 34, 204, DateTimeKind.Utc).AddTicks(293), new DateTime(2022, 12, 5, 11, 14, 34, 204, DateTimeKind.Utc).AddTicks(299), "$2a$11$gbKfZUS2AqtrtS7AEfxQF.OFg.t2GmBe4mJNGVlBG4xp1Vb9GyxoO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ModifiedAt", "Password" },
                values: new object[] { new DateTime(2022, 12, 5, 11, 14, 34, 315, DateTimeKind.Utc).AddTicks(8086), new DateTime(2022, 12, 5, 11, 14, 34, 315, DateTimeKind.Utc).AddTicks(8091), "$2a$11$uuQXyOh.4Eg4CDwYqT4EUuka8bP6O.1UH73dfeCM2wPVcbGtAw11." });
        }
    }
}
