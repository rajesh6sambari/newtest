using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.Models
{
    public class Term
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, DisplayName("Term Start"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateFrom { get; set; }
        [Required, DisplayName("Term End"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateTo { get; set; }
        [Required, DisplayName("Institution")]
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }
    }
}
