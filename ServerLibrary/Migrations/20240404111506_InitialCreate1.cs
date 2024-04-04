using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee4RetailStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee4RetailStores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ioTDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    StoreAreaId = table.Column<int>(type: "int", nullable: false),
                    EmployeesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ioTDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ioTDevices_Employee4RetailStores_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employee4RetailStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ioTDevices_StoreAreas_StoreAreaId",
                        column: x => x.StoreAreaId,
                        principalTable: "StoreAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ioTDevices_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ioTDevices_EmployeesId",
                table: "ioTDevices",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_ioTDevices_StoreAreaId",
                table: "ioTDevices",
                column: "StoreAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ioTDevices_StoreId",
                table: "ioTDevices",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "ioTDevices");

            migrationBuilder.DropTable(
                name: "Employee4RetailStores");

            migrationBuilder.DropTable(
                name: "StoreAreas");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
