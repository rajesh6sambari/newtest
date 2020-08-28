using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class AssignmentTag
    {
        public int Id { get; set; }
        [Required, DisplayName("Tag")]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        [Required, DisplayName("Assignment")]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
    }
}
