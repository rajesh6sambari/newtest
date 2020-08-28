using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        [Required, DisplayName("Assignment Name")]
        public string Name { get; set; }
        [DisplayName("Assignment Specification")]
        public int AssignmentSpecificationId { get; set; }
        [DisplayName("Assignment Specification")]
        public AssignmentSpecification AssignmentSpecification { get; set; }
        public ICollection<InstructorAssignment> Instructors { get; set; } = new List<InstructorAssignment>();
        public ICollection<StudentAssignment> Students { get; set; } = new List<StudentAssignment>();
        [DisplayName("Course")]
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        [DisplayName("Reference Solution")]
        public int ReferenceSolutionId { get; set; }
        [DisplayName("Reference Solution")]
        public ReferenceSolution ReferenceSolution { get; set; }
        [DisplayName("Reference Test Cases Solutions")]
        public int ReferenceTestCasesSolutionsId { get; set; }
        [DisplayName("Reference Test Cases Solutions")]
        public ReferenceTestCasesSolutions ReferenceTestCasesSolutions { get; set; }
        [Required, DisplayName("Language")]
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        [Required, Range(1, 100), DisplayName("Test Coverage Level Threshold (1 - 100)")]
        public double TestCoverageLevel { get; set; }
        [Required, Range(1, 100), DisplayName("Redundant Test Threshold (1 - 100)")]
        public double RedundantTestLevel { get; set; }
        public ICollection<AssignmentApplicationMode> AssignmentApplicationModes { get; set; } = new List<AssignmentApplicationMode>();
        [DisplayName("Coverage Type Option")]
        public ICollection<AssignmentCoverageTypeOption> AssignmentCoverageTypeOptions { get; set; } = new List<AssignmentCoverageTypeOption>();
        [Required, DisplayName("Feedback Level Option")]
        public int FeedbackLevelOptionId { get; set; }
        [DisplayName("Feedback Level Option")]
        public FeedbackLevelOption FeedbackLevelOption { get; set; }
        [Required, DisplayName("Testing Type Option")]
        public int TestingTypeOptionId { get; set; }
        [DisplayName("Coverage Type Option")]
        public TestingTypeOption TestingTypeOption { get; set; }
        [DisplayName("Assignment Visibility Status")]
        public int AssignmentVisibilityProtectionLevelId { get; set; }
        public AssignmentVisibilityProtectionLevel AssignmentVisibilityProtectionLevel { get; set; }
        [Required, DisplayName("Institution")]
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }
        public IList<AssignmentTag> Tags { get; set; } = new List<AssignmentTag>();
        [DisplayName("Difficulty")]
        public int? DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
