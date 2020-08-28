using System.Collections.Generic;
using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Data;
using TestingTutor.Dev.Engine.Utilities;

namespace TestingTutor.Dev.Engine.Generators
{
    public interface IUnitTestGenerator
    {
        Task<ICollection<UnitTestResult>> GenerateResults(SubmissionData data, string snapshot, DevAssignment assignment,
            ICollection<SnapshotMethod> snapshotMethods);

        Task<ICollection<UnitTest>> GenerateResults(PreAssignment assignment, DirectoryHandler handler, string root);
    }
}
