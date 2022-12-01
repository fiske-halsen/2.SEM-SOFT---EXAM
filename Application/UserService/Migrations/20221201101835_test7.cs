using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    public partial class test7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityInfoId",
                        column: x => x.CityInfoId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "City", "ZipCode" },
                values: new object[,]
                {
                    { 1, "Hillerød", "3400" },
                    { 2, "Fredensborg", "3480" },
                    { 3, "Taastrup", "2630" },
                    { 4, "Hedehusene", "2640" },
                    { 5, "Charlottenlund", "2920" },
                    { 6, "CityTest", "3000" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[,]
                {
                    { 1, "Customer" },
                    { 2, "DeliveryPerson" },
                    { 3, "RestaurantOwner" }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "CityInfoId", "StreetName" },
                values: new object[] { 1, 1, "Skovledet" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "CityInfoId", "StreetName" },
                values: new object[] { 2, 5, "Cphbusinessvej" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FirstName", "ModifiedAt", "Password", "RoleId" },
                values: new object[] { 1, 1, new DateTime(2022, 12, 1, 10, 18, 34, 556, DateTimeKind.Utc).AddTicks(9876), "phillip.andersen1999@gmail.com", "Phillip", new DateTime(2022, 12, 1, 10, 18, 34, 556, DateTimeKind.Utc).AddTicks(9878), "$2a$11$GKITrmfmvtCJ/Ta8nNyGe.gjgk16xMHY068siraW.HGZaOTrGWdd.", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FirstName", "ModifiedAt", "Password", "RoleId" },
                values: new object[] { 2, 2, new DateTime(2022, 12, 1, 10, 18, 34, 670, DateTimeKind.Utc).AddTicks(88), "lukasbangstoltz@gmail.com", "Lukas", new DateTime(2022, 12, 1, 10, 18, 34, 670, DateTimeKind.Utc).AddTicks(94), "$2a$11$ZI.9RkjX5VshfvrJpsyJduqzAAwTB8uhxoRAtcliO.gaZAZ5mZX8W", 3 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FirstName", "ModifiedAt", "Password", "RoleId" },
                values: new object[] { 3, 2, new DateTime(2022, 12, 1, 10, 18, 34, 783, DateTimeKind.Utc).AddTicks(5258), "christofferiw@gmail.com", "Christoffer", new DateTime(2022, 12, 1, 10, 18, 34, 783, DateTimeKind.Utc).AddTicks(5264), "$2a$11$firrf1Ua4JJ5xE5qSSnXm.rcKrpCT/06EGtJxEvZI0sthe/2ISwjy", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityInfoId",
                table: "Addresses",
                column: "CityInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
