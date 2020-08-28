using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingTutor.UI.Migrations
{
    public partial class RemoveDescriptionFromTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tags",
                nullable: true);
        }
    }
}
