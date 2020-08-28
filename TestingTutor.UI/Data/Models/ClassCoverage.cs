using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class ClassCoverage
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Container { get; set; }
        public IList<MethodCoverage> MethodCoverages { get; set; } = new List<MethodCoverage>(); 
    }
}
