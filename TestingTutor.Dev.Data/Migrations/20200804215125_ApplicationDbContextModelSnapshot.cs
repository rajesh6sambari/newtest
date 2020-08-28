using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingTutor.Dev.Data.Migrations
{
    public partial class ApplicationDbContextModelSnapshot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentSolutions_AssignmentSolutionId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_CourseClasses_CourseClassId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_TestProjects_TestProjectId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_MarkovModels_Assignments_AssignmentId",
                table: "MarkovModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_Assignments_AssignmentId",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "Filename",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "TestProjectId",
                table: "Assignments",
                newName: "TestingTypeOptionId");

            migrationBuilder.RenameColumn(
                name: "CourseClassId",
                table: "Assignments",
                newName: "ReferenceTestCasesSolutionsId");

            migrationBuilder.RenameColumn(
                name: "AssignmentSolutionId",
                table: "Assignments",
                newName: "ReferenceSolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_TestProjectId",
                table: "Assignments",
                newName: "IX_Assignments_TestingTypeOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_CourseClassId",
                table: "Assignments",
                newName: "IX_Assignments_ReferenceTestCasesSolutionsId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_AssignmentSolutionId",
                table: "Assignments",
                newName: "IX_Assignments_ReferenceSolutionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Assignments",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AddColumn<int>(
                name: "AssignmentSpecificationId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AssignmentVisibilityProtectionLevelId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyId",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackLevelOptionId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RedundantTestLevel",
                table: "Assignments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TestCoverageLevel",
                table: "Assignments",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationModes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(nullable: false),
                    FileBytes = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSpecifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentVisibilityProtectionLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentVisibilityProtectionLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverageTypeOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageTypeOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Filename = table.Column<string>(maxLength: 256, nullable: false),
                    CourseClassId = table.Column<int>(nullable: false),
                    AssignmentSolutionId = table.Column<int>(nullable: false),
                    TestProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevAssignments_AssignmentSolutions_AssignmentSolutionId",
                        column: x => x.AssignmentSolutionId,
                        principalTable: "AssignmentSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevAssignments_CourseClasses_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "CourseClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevAssignments_TestProjects_TestProjectId",
                        column: x => x.TestProjectId,
                        principalTable: "TestProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "EngineException",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Phase = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    Report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineException", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackLevelOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackLevelOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstructorAssignment",
                columns: table => new
                {
                    InstructorAssignmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    InstructorId = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorAssignment", x => x.InstructorAssignmentId);
                    table.ForeignKey(
                        name: "FK_InstructorAssignment_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorAssignment_AspNetUsers_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(nullable: false),
                    FileBytes = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceSolutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceTestCasesSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(nullable: false),
                    FileBytes = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceTestCasesSolutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentAssignment",
                columns: table => new
                {
                    StudentAssignmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAssignment", x => x.StudentAssignmentId);
                    table.ForeignKey(
                        name: "FK_StudentAssignment_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAssignment_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestCaseStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCaseStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestConcepts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    ConceptualContent = table.Column<string>(nullable: false),
                    DetailedContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestConcepts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestingTypeOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingTypeOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentApplicationModes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsChecked = table.Column<bool>(nullable: false),
                    AssignmentId = table.Column<int>(nullable: false),
                    ApplicationModeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentApplicationModes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentApplicationModes_ApplicationModes_ApplicationModeId",
                        column: x => x.ApplicationModeId,
                        principalTable: "ApplicationModes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentApplicationModes_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentCoverageTypeOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsChecked = table.Column<bool>(nullable: false),
                    AssignmentId = table.Column<int>(nullable: false),
                    CoverageTypeOptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentCoverageTypeOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentCoverageTypeOption_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentCoverageTypeOption_CoverageTypeOptions_CoverageTypeOptionId",
                        column: x => x.CoverageTypeOptionId,
                        principalTable: "CoverageTypeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EngineExceptionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_EngineException_EngineExceptionId",
                        column: x => x.EngineExceptionId,
                        principalTable: "EngineException",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    InstitutionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terms_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_AssignmentTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassCoverage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Container = table.Column<string>(nullable: false),
                    FeedbackId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassCoverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassCoverage_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentTestResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TestName = table.Column<string>(nullable: true),
                    EquivalenceClass = table.Column<string>(nullable: true),
                    TestCaseStatusId = table.Column<int>(nullable: false),
                    Passed = table.Column<bool>(nullable: false),
                    FeedbackId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTestResults_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTestResults_TestCaseStatuses_TestCaseStatusId",
                        column: x => x.TestCaseStatusId,
                        principalTable: "TestCaseStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubmitterId = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<int>(nullable: false),
                    ApplicationMode = table.Column<string>(nullable: true),
                    FeedbackId = table.Column<int>(nullable: true),
                    SubmissionDateTime = table.Column<DateTime>(nullable: false),
                    SubmitterSolution = table.Column<byte[]>(nullable: true),
                    SubmitterTestCaseSolution = table.Column<byte[]>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseName = table.Column<string>(nullable: false),
                    TermId = table.Column<int>(nullable: false),
                    InstitutionId = table.Column<int>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodCoverage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    LinesCovered = table.Column<int>(nullable: false),
                    LinesMissed = table.Column<int>(nullable: false),
                    BranchesCovered = table.Column<int>(nullable: false),
                    BranchesMissed = table.Column<int>(nullable: false),
                    ConditionsCovered = table.Column<int>(nullable: false),
                    ConditionsMissed = table.Column<int>(nullable: false),
                    ClassCoverageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodCoverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodCoverage_ClassCoverage_ClassCoverageId",
                        column: x => x.ClassCoverageId,
                        principalTable: "ClassCoverage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstructorTestResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TestName = table.Column<string>(nullable: true),
                    Pass = table.Column<bool>(nullable: false),
                    TestCaseStatusId = table.Column<int>(nullable: false),
                    InstructorTestResultId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorTestResults_StudentTestResults_InstructorTestResultId",
                        column: x => x.InstructorTestResultId,
                        principalTable: "StudentTestResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorTestResults_TestCaseStatuses_TestCaseStatusId",
                        column: x => x.TestCaseStatusId,
                        principalTable: "TestCaseStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResultConcepts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InstructorTestResultId = table.Column<int>(nullable: false),
                    TestConceptId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResultConcepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResultConcepts_StudentTestResults_InstructorTestResultId",
                        column: x => x.InstructorTestResultId,
                        principalTable: "StudentTestResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResultConcepts_TestConcepts_TestConceptId",
                        column: x => x.TestConceptId,
                        principalTable: "TestConcepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorCourses",
                columns: table => new
                {
                    InstructorCourseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InstructorId = table.Column<string>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorCourses", x => x.InstructorCourseId);
                    table.ForeignKey(
                        name: "FK_InstructorCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorCourses_AspNetUsers_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    StudentCourseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<string>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => x.StudentCourseId);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourses_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationModes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Learning Mode" },
                    { 2, "Development Mode" }
                });

            migrationBuilder.InsertData(
                table: "AssignmentVisibilityProtectionLevels",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "The assignment can only be used by the instructor", "Private" },
                    { 2, "The assignment can be used by any instructor belonging to the same organization", "Organization-Only" },
                    { 3, "The assignment can be used by any instructor from any organization", "Repository" }
                });

            migrationBuilder.InsertData(
                table: "CoverageTypeOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Statement" },
                    { 2, "Branch" },
                    { 3, "Condition" }
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Easy" },
                    { 2, "Medium" },
                    { 3, "Hard" }
                });

            migrationBuilder.InsertData(
                table: "FeedbackLevelOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Conceptual - Self-regulating" },
                    { 5, "Conceptual - Process" },
                    { 4, "Conceptual - Task" },
                    { 3, "No Feedback" },
                    { 2, "Detailed Feedback" },
                    { 1, "Conceptual Feedback" }
                });

            migrationBuilder.InsertData(
                table: "Institutions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Oregon Institute of Technology" },
                    { 2, "North Dakota State University" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Python" },
                    { 2, "Java" }
                });

            migrationBuilder.InsertData(
                table: "TestCaseStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Test case matches a reference test case", "Covered" },
                    { 2, "Test case is redundant", "Redundant" },
                    { 3, "A reference test case was not covered", "Uncovered" },
                    { 4, "A reference test case has failed", "Failed" }
                });

            migrationBuilder.InsertData(
                table: "TestingTypeOptions",
                columns: new[] { "Id", "IsChecked", "Name" },
                values: new object[,]
                {
                    { 1, true, "Black Box" },
                    { 2, false, "White Box" }
                });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "Id", "DateFrom", "DateTo", "InstitutionId", "Name" },
                values: new object[] { 1, new DateTime(2019, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Spring 2019" });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "Id", "DateFrom", "DateTo", "InstitutionId", "Name" },
                values: new object[] { 2, new DateTime(2019, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Summer 2019" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseName", "InstitutionId", "IsArchived", "IsPublished", "TermId" },
                values: new object[] { 1, "CST 236", 1, false, false, 1 });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseName", "InstitutionId", "IsArchived", "IsPublished", "TermId" },
                values: new object[] { 2, "CST 211", 1, false, false, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignmentSpecificationId",
                table: "Assignments",
                column: "AssignmentSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignmentVisibilityProtectionLevelId",
                table: "Assignments",
                column: "AssignmentVisibilityProtectionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DifficultyId",
                table: "Assignments",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_FeedbackLevelOptionId",
                table: "Assignments",
                column: "FeedbackLevelOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_InstitutionId",
                table: "Assignments",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_LanguageId",
                table: "Assignments",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InstitutionId",
                table: "AspNetUsers",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentApplicationModes_ApplicationModeId",
                table: "AssignmentApplicationModes",
                column: "ApplicationModeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentApplicationModes_AssignmentId",
                table: "AssignmentApplicationModes",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentCoverageTypeOption_AssignmentId",
                table: "AssignmentCoverageTypeOption",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentCoverageTypeOption_CoverageTypeOptionId",
                table: "AssignmentCoverageTypeOption",
                column: "CoverageTypeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentTag_AssignmentId",
                table: "AssignmentTag",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentTag_TagId",
                table: "AssignmentTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassCoverage_FeedbackId",
                table: "ClassCoverage",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstitutionId",
                table: "Courses",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TermId",
                table: "Courses",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_DevAssignments_AssignmentSolutionId",
                table: "DevAssignments",
                column: "AssignmentSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_DevAssignments_CourseClassId",
                table: "DevAssignments",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_DevAssignments_TestProjectId",
                table: "DevAssignments",
                column: "TestProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_EngineExceptionId",
                table: "Feedback",
                column: "EngineExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorAssignment_AssignmentId",
                table: "InstructorAssignment",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorAssignment_InstructorId",
                table: "InstructorAssignment",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourses_CourseId",
                table: "InstructorCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourses_InstructorId",
                table: "InstructorCourses",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorTestResults_InstructorTestResultId",
                table: "InstructorTestResults",
                column: "InstructorTestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorTestResults_TestCaseStatusId",
                table: "InstructorTestResults",
                column: "TestCaseStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodCoverage_ClassCoverageId",
                table: "MethodCoverage",
                column: "ClassCoverageId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignment_AssignmentId",
                table: "StudentAssignment",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignment_StudentId",
                table: "StudentAssignment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestResults_FeedbackId",
                table: "StudentTestResults",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestResults_TestCaseStatusId",
                table: "StudentTestResults",
                column: "TestCaseStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssignmentId",
                table: "Submissions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_FeedbackId",
                table: "Submissions",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Terms_InstitutionId",
                table: "Terms",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResultConcepts_InstructorTestResultId",
                table: "TestResultConcepts",
                column: "InstructorTestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResultConcepts_TestConceptId",
                table: "TestResultConcepts",
                column: "TestConceptId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Institutions_InstitutionId",
                table: "AspNetUsers",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentSpecifications_AssignmentSpecificationId",
                table: "Assignments",
                column: "AssignmentSpecificationId",
                principalTable: "AssignmentSpecifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentVisibilityProtectionLevels_AssignmentVisibilityProtectionLevelId",
                table: "Assignments",
                column: "AssignmentVisibilityProtectionLevelId",
                principalTable: "AssignmentVisibilityProtectionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_FeedbackLevelOptions_FeedbackLevelOptionId",
                table: "Assignments",
                column: "FeedbackLevelOptionId",
                principalTable: "FeedbackLevelOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Institutions_InstitutionId",
                table: "Assignments",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Languages_LanguageId",
                table: "Assignments",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_ReferenceSolutions_ReferenceSolutionId",
                table: "Assignments",
                column: "ReferenceSolutionId",
                principalTable: "ReferenceSolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_ReferenceTestCasesSolutions_ReferenceTestCasesSolutionsId",
                table: "Assignments",
                column: "ReferenceTestCasesSolutionsId",
                principalTable: "ReferenceTestCasesSolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_TestingTypeOptions_TestingTypeOptionId",
                table: "Assignments",
                column: "TestingTypeOptionId",
                principalTable: "TestingTypeOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MarkovModels_DevAssignments_AssignmentId",
                table: "MarkovModels",
                column: "AssignmentId",
                principalTable: "DevAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_DevAssignments_AssignmentId",
                table: "Snapshots",
                column: "AssignmentId",
                principalTable: "DevAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Institutions_InstitutionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentSpecifications_AssignmentSpecificationId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssignmentVisibilityProtectionLevels_AssignmentVisibilityProtectionLevelId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Difficulties_DifficultyId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_FeedbackLevelOptions_FeedbackLevelOptionId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Institutions_InstitutionId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Languages_LanguageId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_ReferenceSolutions_ReferenceSolutionId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_ReferenceTestCasesSolutions_ReferenceTestCasesSolutionsId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_TestingTypeOptions_TestingTypeOptionId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_MarkovModels_DevAssignments_AssignmentId",
                table: "MarkovModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_DevAssignments_AssignmentId",
                table: "Snapshots");

            migrationBuilder.DropTable(
                name: "AssignmentApplicationModes");

            migrationBuilder.DropTable(
                name: "AssignmentCoverageTypeOption");

            migrationBuilder.DropTable(
                name: "AssignmentSpecifications");

            migrationBuilder.DropTable(
                name: "AssignmentTag");

            migrationBuilder.DropTable(
                name: "AssignmentVisibilityProtectionLevels");

            migrationBuilder.DropTable(
                name: "DevAssignments");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropTable(
                name: "FeedbackLevelOptions");

            migrationBuilder.DropTable(
                name: "InstructorAssignment");

            migrationBuilder.DropTable(
                name: "InstructorCourses");

            migrationBuilder.DropTable(
                name: "InstructorTestResults");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "MethodCoverage");

            migrationBuilder.DropTable(
                name: "ReferenceSolutions");

            migrationBuilder.DropTable(
                name: "ReferenceTestCasesSolutions");

            migrationBuilder.DropTable(
                name: "StudentAssignment");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TestingTypeOptions");

            migrationBuilder.DropTable(
                name: "TestResultConcepts");

            migrationBuilder.DropTable(
                name: "ApplicationModes");

            migrationBuilder.DropTable(
                name: "CoverageTypeOptions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ClassCoverage");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "StudentTestResults");

            migrationBuilder.DropTable(
                name: "TestConcepts");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "TestCaseStatuses");

            migrationBuilder.DropTable(
                name: "Institutions");

            migrationBuilder.DropTable(
                name: "EngineException");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignmentSpecificationId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AssignmentVisibilityProtectionLevelId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_DifficultyId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_FeedbackLevelOptionId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_InstitutionId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_LanguageId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InstitutionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AssignmentSpecificationId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentVisibilityProtectionLevelId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "FeedbackLevelOptionId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "RedundantTestLevel",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "TestCoverageLevel",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TestingTypeOptionId",
                table: "Assignments",
                newName: "TestProjectId");

            migrationBuilder.RenameColumn(
                name: "ReferenceTestCasesSolutionsId",
                table: "Assignments",
                newName: "CourseClassId");

            migrationBuilder.RenameColumn(
                name: "ReferenceSolutionId",
                table: "Assignments",
                newName: "AssignmentSolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_TestingTypeOptionId",
                table: "Assignments",
                newName: "IX_Assignments_TestProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_ReferenceTestCasesSolutionsId",
                table: "Assignments",
                newName: "IX_Assignments_CourseClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_ReferenceSolutionId",
                table: "Assignments",
                newName: "IX_Assignments_AssignmentSolutionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Assignments",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "Assignments",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssignmentSolutions_AssignmentSolutionId",
                table: "Assignments",
                column: "AssignmentSolutionId",
                principalTable: "AssignmentSolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_CourseClasses_CourseClassId",
                table: "Assignments",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_TestProjects_TestProjectId",
                table: "Assignments",
                column: "TestProjectId",
                principalTable: "TestProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MarkovModels_Assignments_AssignmentId",
                table: "MarkovModels",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_Assignments_AssignmentId",
                table: "Snapshots",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
