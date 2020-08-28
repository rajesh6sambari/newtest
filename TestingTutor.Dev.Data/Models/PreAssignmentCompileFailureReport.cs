using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class PreAssignmentCompileFailureReport : PreAssignmentReport
    {
        public PreAssignmentCompileFailureReport()
        {
            Type = PreAssignmentReportTypes.CompileFailure;
        }

        [Required, DisplayName("Compile Failure")]
        public string Report { get; set; }
    }
}
