using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class StudentCourseClass : IdentityModel<int>
    {
        [Required]
        public string StudentId { get; set; }
        public virtual  Student Student { get; set; }
        [Required]
        public int CourseClassId { get; set; }
        public virtual CourseClass Class { get; set; }
    }
}
