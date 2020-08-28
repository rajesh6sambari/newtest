using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingTutor.UI.Migrations
{
    public partial class AddedDifficultytoAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DifficultyId",
                table: "Assignments",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Value" },
                values: new object[] { 1, "Easy" });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Value" },
                values: new object[] { 2, "Medium" });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Value" },
                values: new object[] { 3, "Hard" });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DifficultyId",
                table: "Assignments",
                column: "DifficultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_DifficultyId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "Assignments");
        }
    }
}
