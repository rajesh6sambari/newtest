using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingTutor.UI.Data.Models
{
    public class TestConcept
    {
        public int Id { get; set; }
        [Required, Display(Name="Test Concept")]
        public string Name { get; set; }
        [Required, Display(Name="Conceptual Concept")]
        public string ConceptualContent { get; set; }
        [Display(Name="Detailed Concept")]
        public string DetailedContent { get; set; }
        public ICollection<TestResultConcept> TestResultConcepts { get; set; }
    }
}