using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class FeedbackLevelOption
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
