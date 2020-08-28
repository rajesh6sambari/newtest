using System.Collections.Generic;

namespace TestingTutor.Dev.Data.Models
{
    public class PreAssignmentSucessReport : PreAssignmentReport
    {
        public PreAssignmentSucessReport()
        {
            Type = PreAssignmentReportTypes.Success;
        }
    }
}
