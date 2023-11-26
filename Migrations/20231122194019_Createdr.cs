using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _16noyabr.Migrations
{
    public partial class Createdr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Slides",
                newName: "Images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Images",
                table: "Slides",
                newName: "Image");
        }
    }
}
