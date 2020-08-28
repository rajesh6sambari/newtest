using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class SnapshotFailureReport : SnapshotReport
    {
        public SnapshotFailureReport()
        {
            Type = SnapshotReportTypes.Failure;
        }

        [Required, DisplayName("Failure Report")]
        public string Report { get; set; }
    }
}
