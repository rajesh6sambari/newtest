using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingTutor.UI.Migrations
{
    public partial class AddedAssignmentTagforManytoManyReleationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Assignments_AssignmentId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_AssignmentId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Tag");

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyId",
                table: "Assignments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "AssignmentTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TagId = table.Column<int>(nullable: false),
                    AssignmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentTag_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentTag_AssignmentId",
                table: "AssignmentTag",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentTag_TagId",
                table: "AssignmentTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments");

            migrationBuilder.DropTable(
                name: "AssignmentTag");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Tag",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyId",
                table: "Assignments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_AssignmentId",
                table: "Tag",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Assignments_AssignmentId",
                table: "Tag",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
