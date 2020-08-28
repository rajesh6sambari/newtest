using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required, DisplayName("Course Name")]
        public string CourseName { get; set; }
        [Required, DisplayName("Term")]
        public int TermId { get; set; }
        public Term Term { get; set; }
        [DisplayName("Institution")]
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }
        [Display(Name = "Archive Course")]
        public bool IsArchived { get; set; }
        [Display(Name ="Publish Course")]
        public bool IsPublished { get; set; }
        public ICollection<InstructorCourse> Instructors { get; set; } = new List<InstructorCourse>();
        public ICollection<StudentCourse> Students { get; set; } = new List<StudentCourse>();
    }
}
