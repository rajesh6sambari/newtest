using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [Display(Name="Course ID/Name")]
        public string CourseName { get; set; }
        [Display(Name = "Term")]
        public string TermName { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}
