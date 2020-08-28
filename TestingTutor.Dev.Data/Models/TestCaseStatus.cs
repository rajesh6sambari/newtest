using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class TestCaseStatus
    { 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public override string ToString() => Name;
    }
}
