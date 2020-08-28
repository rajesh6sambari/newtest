using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class AssignmentVisibilityProtectionLevel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
