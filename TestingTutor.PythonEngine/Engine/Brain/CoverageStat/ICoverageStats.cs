using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Models;

namespace TestingTutor.PythonEngine.Engine.Brain.CoverageStat
{
    public interface ICoverageStats
    {
        void GetStats(SubmissionDto submission, ref EngineWorkingDirectories workingDirectories,
            ref FeedbackDto feedback);
    }
}
