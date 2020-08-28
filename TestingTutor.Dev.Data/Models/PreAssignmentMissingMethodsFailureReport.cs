using System.Collections.Generic;

namespace TestingTutor.Dev.Data.Models
{
    public class PreAssignmentMissingMethodsFailureReport : PreAssignmentReport
    {
        public PreAssignmentMissingMethodsFailureReport()
        {
            Type = PreAssignmentReportTypes.MissingMethodsFailure;
        }

        public virtual ICollection<MethodDeclaration> MissingMethodDeclarations { get; set; } = new List<MethodDeclaration>();
    }
}
