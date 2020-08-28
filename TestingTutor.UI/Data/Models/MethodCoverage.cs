using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class MethodCoverage
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int LinesCovered { get; set; }
        [Required]
        public int LinesMissed { get; set; }
        [Required]
        public int BranchesCovered { get; set; }
        [Required]
        public int BranchesMissed { get; set; }
        [Required]
        public int ConditionsCovered { get; set; }
        [Required]
        public int ConditionsMissed { get; set; }
    }
}