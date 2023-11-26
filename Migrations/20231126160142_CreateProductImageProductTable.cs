using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _16noyabr.Migrations
{
    public partial class CreateProductImageProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SKU",
                table: "Products",
                newName: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Products",
                newName: "SKU");
        }
    }
}
