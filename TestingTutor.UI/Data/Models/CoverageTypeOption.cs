using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class CoverageTypeOption
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ICollection<AssignmentCoverageTypeOption> AssignmentCoverageTypeOptions { get; set; } = new List<AssignmentCoverageTypeOption>();
    }
}
