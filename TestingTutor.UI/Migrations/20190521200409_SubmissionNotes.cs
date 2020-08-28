using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingTutor.UI.Migrations
{
    public partial class SubmissionNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Submissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Submissions");
        }
    }
}
