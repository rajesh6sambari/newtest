using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingTutor.UI.Data.Models
{
    public class StudentCourse
    {
        public int StudentCourseId { get; set; }
        [Required, ForeignKey("ApplicationUser")]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        [Required, ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
