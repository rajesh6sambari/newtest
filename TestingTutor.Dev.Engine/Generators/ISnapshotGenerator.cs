using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Data;

namespace TestingTutor.Dev.Engine.Generators
{
    public interface ISnapshotGenerator
    {
        Task<IList<Snapshot>> Generate(SubmissionData data, DevAssignment assignment);
    }
}
