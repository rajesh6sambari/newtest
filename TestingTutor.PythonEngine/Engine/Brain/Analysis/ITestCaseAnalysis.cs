using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Models;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis
{
    public interface ITestCaseAnalysis
    {
        void Run(SubmissionDto submissionDTO, ref EngineWorkingDirectories workingDirectories,
            ref FeedbackDto feedback);
    }
}
