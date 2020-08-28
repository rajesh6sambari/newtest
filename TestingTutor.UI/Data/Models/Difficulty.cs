using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
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
