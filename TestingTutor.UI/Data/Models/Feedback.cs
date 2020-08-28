using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestingTutor.EngineModels;

namespace TestingTutor.UI.Data.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int? EngineExceptionId { get; set; }
        public EngineException EngineException { get; set; }
        public ICollection<ClassCoverage> ClassCoverages { get; set; } = new List<ClassCoverage>();
        public ICollection<InstructorTestResult> InstructorTestResults { get; set; } = new List<InstructorTestResult>();
    }
}
