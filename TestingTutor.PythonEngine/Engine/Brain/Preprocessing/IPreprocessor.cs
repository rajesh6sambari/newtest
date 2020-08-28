using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Models;

namespace TestingTutor.PythonEngine.Engine.Brain.Preprocessing
{
    public interface IPreprocessor
    {
        bool Preprocessing(SubmissionDto submissionDTO, string directory, out EngineWorkingDirectories workingDirectories,
            out FeedbackDto feedback);
    }
}
