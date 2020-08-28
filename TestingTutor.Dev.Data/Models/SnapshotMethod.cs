using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class SnapshotMethod : IdentityModel<int>
    {
        [Required, DisplayName("Method Declared")]
        public bool Declared { get; set; }

        [Required, DisplayName("Method Declaration")]
        public int MethodDeclarationId { get; set; }
        public virtual MethodDeclaration MethodDeclaration { get; set; }
        [DisplayName("Code Analysis Metric")]
        public int? CodeAnalysisMetricId { get; set; }
        public virtual CodeAnalysisMetric CodeAnalysisMetric { get; set; }
    }
}
