using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantService.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
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
                        name: "FK_Addresses_CityInfos_CityInfoId",
                        column: x => x.CityInfoId,
                        principalTable: "CityInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurants_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CityInfos",
                columns: new[] { "Id", "City", "ZipCode" },
                values: new object[] { 1, "Gentofte", "2920" });

            migrationBuilder.InsertData(
                table: "Menus",
                column: "Id",
                value: 1);

            migrationBuilder.InsertData(
                table: "Menus",
                column: "Id",
                value: 2);

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "CityInfoId", "StreetName" },
                values: new object[,]
                {
                    { 1, 1, "Skovvej" },
                    { 2, 1, "Hovmarksvej" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Description", "MenuId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "wow smager godt", 1, "salatpizza", 79.989999999999995 },
                    { 2, "wow smager godt", 1, "Peperoni", 79.230000000000004 },
                    { 3, "wow smager godt", 1, "Calzone", 89.989999999999995 },
                    { 4, "wow smager godt", 1, "ChokoladeIs", 39.990000000000002 },
                    { 5, "wow smager godt", 1, "vaniljeis", 39.990000000000002 },
                    { 6, "wow smager godt", 1, "chokoladekage", 39.990000000000002 },
                    { 7, "wow smager godt", 1, "Cola", 19.989999999999998 },
                    { 8, "wow smager godt", 1, "Fanta", 19.989999999999998 },
                    { 9, "wow smager godt", 1, "Mayo", 9.9900000000000002 },
                    { 10, "wow smager godt", 1, "Ketchup", 9.9900000000000002 }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AddressId", "MenuId", "Name", "OwnerId" },
                values: new object[] { 1, 1, 1, "PizzaPusheren", 1 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AddressId", "MenuId", "Name", "OwnerId" },
                values: new object[] { 2, 2, 2, "SushiSlyngeren", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityInfoId",
                table: "Addresses",
                column: "CityInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuId",
                table: "MenuItems",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_AddressId",
                table: "Restaurants",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_MenuId",
                table: "Restaurants",
                column: "MenuId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "CityInfos");
        }
    }
}
