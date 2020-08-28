using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class Difficulty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Difficulty")]
        public string Value { get; set; }
    }
}
