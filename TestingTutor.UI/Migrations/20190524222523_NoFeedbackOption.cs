using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingTutor.UI.Migrations
{
    public partial class NoFeedbackOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FeedbackLevelOptions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "No Feedback" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeedbackLevelOptions",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
