using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class MarkovModel : IdentityModel<int>
    {
        [Required, DisplayName("Publish Date")]
        public DateTime Publish { get; set; } = DateTime.Now;
        [Required, DisplayName("Finished")]
        public bool Finished { get; set; } = false;
        [Required]
        public int AssignmentId { get; set; }
        public virtual DevAssignment Assignment { get; set; }
        [DisplayName("Markov Model's State")]
        public virtual ICollection<MarkovModelState> States { get; set; } = new List<MarkovModelState>();
    }
}
