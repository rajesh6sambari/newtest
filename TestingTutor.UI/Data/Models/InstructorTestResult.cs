using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class InstructorTestResult
    {
        public int Id { get; set; }
        [Display(Name="Test Name")]
        public string TestName { get; set; }
        [Display(Name = "Equivalence Class")] public string EquivalenceClass { get; set; }
        public ICollection<StudentTestResult> StudentTestResults { get; set; } = new List<StudentTestResult>();
        public ICollection<TestResultConcept> TestResultConcepts { get; set; } = new List<TestResultConcept>();
        [Display(Name = "Test Case Status ID")]
        public int TestCaseStatusId { get; set; }
        [Display(Name = "Test Case Status")]
        public TestCaseStatus TestCaseStatus { get; set; }
        [Display(Name = "Test Status")]
        public bool Passed { get; set; }
    }
}
