using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class StudentTestResult
    {
        public int Id { get; set; }
        [Display(Name = "Test Name")]
        public string TestName { get; set; }
        [Display(Name = "Test Case Pass")]
        public bool Pass { get; set; }
        [Display(Name = "Test Case Status ID")]
        public int TestCaseStatusId { get; set; }
        [Display(Name = "Test Case Status")]
        public TestCaseStatus TestCaseStatus { get; set; }
    }
}
