using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.Dev.Data.Models
{
    public class ApplicationMode
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ICollection<AssignmentApplicationMode> AssignmentApplicationModes { get; set; } = new List<AssignmentApplicationMode>(); 
    }
}
