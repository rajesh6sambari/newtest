using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestingTutor.UI.Data.Models;
using TestingTutor.Dev.Data.Models;
using AssignmentApplicationMode = TestingTutor.UI.Data.Models.AssignmentApplicationMode;

namespace TestingTutor.Models
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
