using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Models;
using TestingTutor.UI.Data.Models;

namespace TestingTutor.UI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<PreAssignment> PreAssignments { get; set; }
        public DbSet<PreAssignmentReport> PreAssignmentReports { get; set; }
        public DbSet<PreAssignmentPendingReport> PreAssignmentPendingReports { get; set; }
        public DbSet<PreAssignmentSucessReport> PreAssignmentSucessReports { get; set; }
        public DbSet<PreAssignmentNoFileFailureReport> PreAssignmentNoFileFailureReports { get; set; }
        public DbSet<PreAssignmentCompileFailureReport> PreAssignmentCompileFailureReports { get; set; }
        public DbSet<PreAssignmentBuildFailureReport> PreAssignmentBuildFailureReports { get; set; }
        public DbSet<PreAssignmentNoClassFailureReport> PreAssignmentNoClassFailureReports { get; set; }
        public DbSet<PreAssignmentMissingMethodsFailureReport> PreAssignmentMissingMethodsFailureReports { get; set; }
        public DbSet<PreAssignmentBadTestFolderReport> PreAssignmentBadTestFolderReports { get; set; }
        public DbSet<PreAssignmentFailTestsFailureReport> PreAssignmentFailTestsFailureReports { get; set; }
        public DbSet<AbstractSyntaxTreeMetric> AbstractSyntaxTreeMetrics { get; set; }
        public DbSet<TestingTutor.Dev.Data.Models.DevAssignment> DevAssignments { get; set; }
        public DbSet<AssignmentSolution> AssignmentSolutions { get; set; }
        public DbSet<BagOfWordsMetric> BagOfWordsMetrics { get; set; }
        public DbSet<CodeAnalysisMetric> CodeAnalysisMetrics { get; set; }
        public DbSet<CourseClass> CourseClasses { get; set; }
        public DbSet<MarkovModel> MarkovModels { get; set; }
        public DbSet<MarkovModelState> MarkovModelStates { get; set; }
        public DbSet<MarkovModelTransition> MarkovModelTransitions { get; set; }
        public DbSet<MethodDeclaration> MethodDeclarations { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<SnapshotFailureReport> SnapshotFailureReports { get; set; }
        public DbSet<SnapshotMethod> SnapshotMethods { get; set; }
        public DbSet<SnapshotReport> SnapshotReports { get; set; }
        public DbSet<SnapshotSuccessReport> SnapshotSuccessReports { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourseClass> StudentCourseClasses { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyAnswerQualitative> SurveyAnswerQualitatives { get; set; }
        public DbSet<SurveyAnswerRate> SurveyAnswerRates { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyQuestionQualitative> SurveyQuestionQualitatives { get; set; }
        public DbSet<SurveyQuestionRate> SurveyQuestionRates { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<TestProject> TestProjects { get; set; }
        public DbSet<UnitTest> UnitTests { get; set; }
        public DbSet<UnitTestResult> UnitTestResults { get; set; }
        public DbSet<ApplicationMode> ApplicationModes { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSpecification> AssignmentSpecifications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CoverageTypeOption> CoverageTypeOptions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ReferenceSolution> ReferenceSolutions { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<TestingTypeOption> TestingTypeOptions { get; set; }
        public DbSet<ReferenceTestCasesSolutions> ReferenceTestCasesSolutions { get; set; }
        public DbSet<FeedbackLevelOption> FeedbackLevelOptions { get; set; }
        public DbSet<TestCaseStatus> TestCaseStatuses { get; set; }
        public DbSet<AssignmentVisibilityProtectionLevel> AssignmentVisibilityProtectionLevels { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<InstructorCourse> InstructorCourses { get; set; }
        public DbSet<AssignmentApplicationMode> AssignmentApplicationModes { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<InstructorTestResult> StudentTestResults { get; set; }
        public DbSet<StudentTestResult> InstructorTestResults { get; set; }
        public DbSet<TestConcept> TestConcepts { get; set; }
        public DbSet<TestResultConcept> TestResultConcepts { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<SnapshotReport>()
                .ToTable("SnapshotReport")
                .HasDiscriminator(x => x.Type)
                .HasValue<SnapshotSuccessReport>(SnapshotReport.SnapshotReportTypes.Success)
                .HasValue<SnapshotFailureReport>(SnapshotReport.SnapshotReportTypes.Failure);

            builder.Entity<SurveyQuestion>()
                .ToTable("SurveyQuestion")
                .HasDiscriminator(x => x.Type)
                .HasValue<SurveyQuestionRate>(SurveyQuestion.SurveyQuestionTypes.Rate)
                .HasValue<SurveyQuestionQualitative>(SurveyQuestion.SurveyQuestionTypes.Qualitative);

            builder.Entity<SurveyAnswer>()
                .ToTable("SurveyAnswer")
                .HasDiscriminator(x => x.Type)
                .HasValue<SurveyAnswerQualitative>(SurveyAnswer.SurveyAnswerTypes.Qualitative)
                .HasValue<SurveyAnswerRate>(SurveyAnswer.SurveyAnswerTypes.Rate);

            builder.Entity<PreAssignmentReport>()
                .ToTable("PreAssignmentReport")
                .HasDiscriminator(x => x.Type)
                .HasValue<PreAssignmentPendingReport>(PreAssignmentReport.PreAssignmentReportTypes.Pending)
                .HasValue<PreAssignmentSucessReport>(PreAssignmentReport.PreAssignmentReportTypes.Success)
                .HasValue<PreAssignmentNoFileFailureReport>(PreAssignmentReport.PreAssignmentReportTypes.NoFileFailure)
                .HasValue<PreAssignmentCompileFailureReport>(
                    PreAssignmentReport.PreAssignmentReportTypes.CompileFailure)
                .HasValue<PreAssignmentBuildFailureReport>(PreAssignmentReport.PreAssignmentReportTypes.BuildFailure)
                .HasValue<PreAssignmentNoClassFailureReport>(
                    PreAssignmentReport.PreAssignmentReportTypes.NoClassFailure)
                .HasValue<PreAssignmentMissingMethodsFailureReport>(PreAssignmentReport.PreAssignmentReportTypes
                    .MissingMethodsFailure)
                .HasValue<PreAssignmentFailTestsFailureReport>(PreAssignmentReport.PreAssignmentReportTypes
                    .FailTestsFailure)
                .HasValue<PreAssignmentBadTestFolderReport>(PreAssignmentReport.PreAssignmentReportTypes.BadTestFolder);

            builder.Entity<Survey>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Student>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
            base.OnModelCreating(builder);

            GetSeedingTerms().ToList().ForEach(term => builder.Entity<Term>().HasData(term));
            GetSeedingCourses().ToList().ForEach(course => builder.Entity<Course>().HasData(course));
            GetSeedingApplicationModes().ToList().ForEach(mode => builder.Entity<ApplicationMode>().HasData(mode));
            GetSeedingFeedbackLevelOptions().ToList().ForEach(option => builder.Entity<FeedbackLevelOption>().HasData(option));
            GetSeedingLanguages().ToList().ForEach(lang => builder.Entity<Language>().HasData(lang));
            GetSeedingCoverageTypeOptions().ToList().ForEach(option => builder.Entity<CoverageTypeOption>().HasData(option));
            GetSeedingTestingTypeOptions().ToList().ForEach(option => builder.Entity<TestingTypeOption>().HasData(option));
            GetSeedingTestCaseStatuses().ToList().ForEach(option => builder.Entity<TestCaseStatus>().HasData(option));
            GetSeedingAssignmentVisibilityProtectionLevel().ToList().ForEach(option => builder.Entity<AssignmentVisibilityProtectionLevel>().HasData(option));
            GetSeedingInstitutions().ToList().ForEach(institution => builder.Entity<Institution>().HasData(institution));
            GetSeedingDifficulties().ToList().ForEach(difficulty => builder.Entity<Difficulty>().HasData(difficulty));
        }

        public async virtual Task AddAssignment(Assignment assignment)
        {
            await Assignments.AddAsync(assignment);
        }

        public async virtual Task<IList<Course>> GetCoursesAsync()
        {
            return await Courses
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<IList<Assignment>> GetAssignmentsAsync()
        {
            //return await Assignments
            //    .AsNoTracking()
            //    .ToListAsync();
            var assignments = new List<Assignment>();
            foreach (var assignment in Assignments.AsNoTracking().ToList())
            {
                assignments.Add(await GetAssignmentById(assignment.Id));
            }

            return assignments;
        }

        public async virtual Task<IList<ApplicationMode>> GetApplicationModesAsync()
        {
            return await ApplicationModes
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<IList<CoverageTypeOption>> GetCoverageTypeOptionsAsync()
        {
            return await CoverageTypeOptions
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<Assignment> GetAssignmentById(int? id)
        {
            if (id == null) return null;

            var assignment = await Assignments.FindAsync(id);
            Entry(assignment).Reference(x => x.AssignmentSpecification).Load();
            Entry(assignment).Reference(x => x.Course).Query().Load();
            Entry(assignment).Reference(x => x.Language).Load();
            Entry(assignment).Reference(x => x.Institution).Load();
            Entry(assignment).Reference(x => x.Difficulty).Load();
            Entry(assignment).Reference(x => x.AssignmentVisibilityProtectionLevel).Load();
            Entry(assignment).Collection(x => x.Instructors).Query().Include(x => x.Instructor).Load();
            Entry(assignment).Reference(x => x.ReferenceSolution).Load();
            Entry(assignment).Reference(x => x.ReferenceTestCasesSolutions).Load();
            Entry(assignment).Collection(x => x.AssignmentApplicationModes).Load();
            Entry(assignment).Collection(x => x.AssignmentApplicationModes).Query().Include(y => y.ApplicationMode).Load();
            Entry(assignment).Collection(x => x.AssignmentCoverageTypeOptions).Query().Include(y => y.CoverageTypeOption).Load();
            Entry(assignment).Reference(x => x.FeedbackLevelOption).Load();
            Entry(assignment).Reference(x => x.TestingTypeOption).Load();
            Entry(assignment).Collection(x => x.Instructors).Query().Include(a => a.Instructor).Load();
            Entry(assignment).Collection(x => x.Tags).Query().Include(a => a.Tag).Load();

            return assignment;
        }

        public async virtual Task<byte[]> GetDocumentFileBytes(int id, string documentType)
        {
            var assignment = await Assignments.FindAsync(id);
            Entry(assignment).Reference(x => x.AssignmentSpecification).Load();
            Entry(assignment).Reference(x => x.ReferenceSolution).Load();
            Entry(assignment).Reference(x => x.ReferenceTestCasesSolutions).Load();

            switch (documentType)
            {
                case "AssignmentSpecification":
                    return assignment.AssignmentSpecification.FileBytes;
                case "ReferenceSolution":
                    return assignment.ReferenceSolution.FileBytes;
                case "ReferenceTestCasesSolutions":
                    return assignment.ReferenceTestCasesSolutions.FileBytes;
            }

            return null;
        }

        public static IList<Institution> GetSeedingInstitutions()
        {
            return new List<Institution>()
            {
                new Institution()
                {
                    Id = 1,
                    Name = "Oregon Institute of Technology"
                },
                new Institution()
                {
                    Id = 2,
                    Name = "North Dakota State University"
                }
            };
        }

        public static IList<Difficulty> GetSeedingDifficulties()
        {
            return new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = 1,
                    Value = "Easy",
                },
                new Difficulty()
                {
                    Id = 2,
                    Value = "Medium",
                },
                new Difficulty()
                {
                    Id = 3,
                    Value = "Hard",
                }
            };
        }

        public static IList<Term> GetSeedingTerms()
        {
            return new List<Term>()
            {
                new Term
                {
                    Id = 1,
                    Name = "Spring 2019",
                    DateFrom = new DateTime(2019, 03, 01),
                    DateTo = new DateTime(2019, 06, 10),
                    InstitutionId = 1
                },
                new Term
                {
                    Id = 2,
                    Name = "Summer 2019",
                    DateFrom = new DateTime(2019, 06, 20),
                    DateTo = new DateTime(2019, 08, 30),
                    InstitutionId = 1
                }
            };
        }

        public static IList<Course> GetSeedingCourses()
        {
            return new List<Course>()
            {
                new Course()
                {
                    Id = 1,
                    CourseName = "CST 236",
                    TermId = 1,
                    InstitutionId = 1
                    
                },
                new Course()
                {
                    Id = 2,
                    CourseName = "CST 211",
                    TermId = 2,
                    InstitutionId = 1
                }
            };
        }

        public static IList<ApplicationMode> GetSeedingApplicationModes()
        {
            return new List<ApplicationMode>()
            {
                new ApplicationMode {Id = 1, Name = "Learning Mode" },
                new ApplicationMode {Id = 2, Name = "Development Mode" }
            };
        }

        public static IList<FeedbackLevelOption> GetSeedingFeedbackLevelOptions()
        {
            return new List<FeedbackLevelOption>()
            {
                new FeedbackLevelOption {Id = 1, Name = "Conceptual Feedback"},
                new FeedbackLevelOption {Id = 2, Name = "Detailed Feedback"},
                new FeedbackLevelOption {Id = 3, Name = "No Feedback" },
                new FeedbackLevelOption {Id = 4, Name = "Conceptual - Task" },
                new FeedbackLevelOption {Id = 5, Name = "Conceptual - Process" },
                new FeedbackLevelOption {Id = 6, Name = "Conceptual - Self-regulating" }
            };
        }

        public static IList<Language> GetSeedingLanguages()
        {
            return new List<Language>()
            {
                new Language {Id = 1, Name = "Python"},
                new Language {Id = 2, Name = "Java"}
            };
        }

        public static IList<CoverageTypeOption> GetSeedingCoverageTypeOptions()
        {
            return new List<CoverageTypeOption>()
            {
                new CoverageTypeOption() {Id = 1, Name = "Statement" },
                new CoverageTypeOption() {Id = 2, Name = "Branch" },
                new CoverageTypeOption() {Id = 3, Name = "Condition" }
            };
        }

        public static IList<TestingTypeOption> GetSeedingTestingTypeOptions()
        {
            return new List<TestingTypeOption>()
            {
                new TestingTypeOption() {Id = 1, Name = "Black Box", IsChecked = true},
                new TestingTypeOption() { Id = 2, Name = "White Box", IsChecked = false}
            };
        }

        public static IList<TestCaseStatus> GetSeedingTestCaseStatuses()
        {
            return new List<TestCaseStatus>()
            {
                new TestCaseStatus()
                {
                    Id = 1,
                    Name = "Covered",
                    Description = "Test case matches a reference test case"
                },
                new TestCaseStatus()
                {
                    Id = 2,
                    Name = "Redundant",
                    Description = "Test case is redundant"
                },
                new TestCaseStatus()
                {
                    Id = 3,
                    Name = "Uncovered",
                    Description = "A reference test case was not covered"
                },
                new TestCaseStatus()
                {
                    Id = 4,
                    Name = "Failed",
                    Description = "A reference test case has failed"
                },
            };
        }

        public static IList<AssignmentVisibilityProtectionLevel> GetSeedingAssignmentVisibilityProtectionLevel()
        {
            return new List<AssignmentVisibilityProtectionLevel>()
            {
                new AssignmentVisibilityProtectionLevel()
                {
                    Id = 1,
                    Name = "Private",
                    Description = "The assignment can only be used by the instructor"
                },
                new AssignmentVisibilityProtectionLevel()
                {
                    Id = 2,
                    Name = "Organization-Only",
                    Description = "The assignment can be used by any instructor belonging to the same organization"
                },
                new AssignmentVisibilityProtectionLevel()
                {
                    Id = 3,
                    Name = "Repository",
                    Description = "The assignment can be used by any instructor from any organization"
                }
            };
        }

        public static IList<Assignment> GetSeedingAssignments()
        {
            return new List<Assignment>()
            {
                new Assignment
                {
                    Id = 1,
                    InstitutionId = 1,
                    AssignmentApplicationModes = new List<AssignmentApplicationMode>()
                    {
                        new AssignmentApplicationMode
                        {
                            Id = 1,
                            ApplicationModeId = 1,
                            AssignmentId = 1,
                            ApplicationMode = new ApplicationMode {Id = 1, Name = "Learning Mode"},
                        },
                        new AssignmentApplicationMode
                        {
                            Id = 2,
                            ApplicationModeId = 2,
                            AssignmentId = 1,
                            ApplicationMode = new ApplicationMode {Id = 2, Name = "Development Mode"}
                        }
                    },
                    AssignmentSpecificationId = 1,
                    AssignmentSpecification = new AssignmentSpecification
                    {
                        Id = 1,
                        FileName = "File.pdf",
                        FileBytes = new byte[] { new byte() }
                    },
                    CourseId = 1,
                    Course = new Course
                    {
                        Id = 1
                    },
                    AssignmentCoverageTypeOptions = new List<AssignmentCoverageTypeOption>()
                    {
                        new AssignmentCoverageTypeOption
                        {
                            Id = 1,
                            AssignmentId = 1,
                            CoverageTypeOptionId = 1,
                            CoverageTypeOption = new CoverageTypeOption {Id = 1, Name = "Statement"}
                        },
                        new AssignmentCoverageTypeOption
                        {
                            Id = 2,
                            AssignmentId = 1,
                            CoverageTypeOptionId = 2,
                            CoverageTypeOption = new CoverageTypeOption {Id = 2, Name = "Branch"}
                        },
                        new AssignmentCoverageTypeOption
                        {
                            Id = 1,
                            AssignmentId = 1,
                            CoverageTypeOptionId = 1,
                            CoverageTypeOption = new CoverageTypeOption {Id = 1, Name = "Condition"},
                        }
                    },
                    FeedbackLevelOptionId = 1,
                    FeedbackLevelOption = new FeedbackLevelOption
                    {
                        Id = 1
                    },
                    Instructors = new List<InstructorAssignment>
                    {
                        new InstructorAssignment
                        {
                            Instructor = new ApplicationUser(),
                            AssignmentId = 1
                        }
                    },
                    ReferenceTestCasesSolutionsId = 1,
                    ReferenceTestCasesSolutions = new ReferenceTestCasesSolutions
                    {
                        Id = 1,
                        FileName = "File.zip",
                        FileBytes = new byte[] { new byte() }
                    },
                    ReferenceSolutionId = 1,
                    ReferenceSolution = new ReferenceSolution
                    {
                        Id = 1,
                        FileName = "File.zip",
                        FileBytes = new byte[] { new byte() }
                    },
                    AssignmentVisibilityProtectionLevelId = 1
                }
            };
        }
    }
}
