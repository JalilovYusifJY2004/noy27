using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _16noyabr.Migrations
{
    public partial class CreatedTableyeni : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Urrl",
                table: "ProductImages",
                newName: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ProductImages",
                newName: "Urrl");
        }
    }
}
