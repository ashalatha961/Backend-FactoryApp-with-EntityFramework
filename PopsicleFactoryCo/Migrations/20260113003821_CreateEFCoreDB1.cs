using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PopsicleFactoryCo.Migrations
{
    /// <inheritdoc />
    public partial class CreateEFCoreDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Popsicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flavour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popsicles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Popsicles",
                columns: new[] { "Id", "Flavour", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "Chocolate", "Nicollete path hole", 15, 15 },
                    { 2, "Vanilla", "Durham style", 10, 10 },
                    { 3, "Strawberry", "Duval style", 20, 20 },
                    { 4, "Orange", "Deccan style", 25, 25 },
                    { 5, "Blueberry", "Boulevard style", 20, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Popsicles");
        }
    }
}
