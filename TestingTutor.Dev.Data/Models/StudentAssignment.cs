using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class StudentAssignment
    {
        public int StudentAssignmentId { get; set; }
        [Required]
        public int ApplicationUserId { get; set; }
        public ApplicationUser Student { get; set; }
        [Required]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
    }
}