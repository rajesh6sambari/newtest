using System.ComponentModel.DataAnnotations;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.Dev.Data.Models
{
    public class AssignmentApplicationMode
    {
        public int Id { get; set; }
        [Required] public bool IsChecked { get; set; }
        [Required] public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        [Required] public int ApplicationModeId { get; set; }
        public ApplicationMode ApplicationMode { get; set; }
    }
}
