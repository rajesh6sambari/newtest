using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingTutor.UI.Migrations
{
    public partial class AddedAssignmentIdToFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Feedback",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_AssignmentId",
                table: "Feedback",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Assignments_AssignmentId",
                table: "Feedback",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Assignments_AssignmentId",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_AssignmentId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Feedback");
        }
    }
}
