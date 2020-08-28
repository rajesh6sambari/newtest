using System.Collections.Generic;
using System.Threading.Tasks;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Options;
using TestingTutor.Dev.Engine.Utilities;

namespace TestingTutor.Dev.Engine.Generators
{
    public interface IMarkovModelGenerator
    {
        Task Generate(IList<Snapshot> snapshots, MarkovModelOptions options, DirectoryHandler handler, DevAssignment assignment);
    }
}
