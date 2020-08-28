using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class TestingTypeOption
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsChecked { get; set; }
    }
}
