using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class InstructorAssignment
    {
        public int InstructorAssignmentId { get; set; }
        [Required]
        public int ApplicationUserId { get; set; }
        public ApplicationUser Instructor { get; set; }
        [Required]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
    }
}