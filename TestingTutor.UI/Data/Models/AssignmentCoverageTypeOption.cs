using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class AssignmentCoverageTypeOption
    {
        public int Id { get; set; }
        [Required] public bool IsChecked { get; set; }
        [Required] public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        [Required] public int CoverageTypeOptionId { get; set; }
        public CoverageTypeOption CoverageTypeOption { get; set; }
    }
}
