using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingTutor.Dev.Data.Models
{
    public class InstructorCourse
    {
        public int InstructorCourseId { get; set; }
        [Required, ForeignKey("ApplicationUser")]
        public string InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; }
        [Required, ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
