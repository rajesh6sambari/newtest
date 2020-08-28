using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Analysis.AbstractSyntaxTree;
using TestingTutor.Dev.Engine.Data;

namespace TestingTutor.Dev.Engine.Generators
{
    public interface ISnapshotReportGenerator
    {
        Task<SnapshotReport> Generate(SubmissionData data, string snapshot, DevAssignment assignment, AbstractSyntaxTreeNode solution);
    }
}
