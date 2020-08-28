using System.IO;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Factory;
using TestingTutor.PythonEngine.Engine.Models;

namespace TestingTutor.PythonEngine.Engine.Brain.Preprocessing
{
    public class LearningPreprocessor : IPreprocessor
    {
        protected IEngineFactory Factory;

        public LearningPreprocessor(IEngineFactory factory)
        {
            Factory = factory;
        }

        public bool Preprocessing(SubmissionDto submissionDto, string directory, out EngineWorkingDirectories workingDirectories,
            out FeedbackDto feedback)
        {
            workingDirectories = new EngineWorkingDirectories();

            if (!ValidateSubmission(submissionDto, out feedback))
            {
                return false;
            }

            var handler = Factory.FileHandler();

            workingDirectories.ParentDirectory = directory;
            workingDirectories.ReferenceTestSuit = Path.Combine(directory, "ReferenceTestSuit");
            workingDirectories.Solution = Path.Combine(directory, submissionDto.SolutionFolderName);
            workingDirectories.StudentTestSuit = Path.Combine(directory, "StudentTestSuit");

            handler.UnzipByteArray(submissionDto.ReferenceTestSolution, directory,
                workingDirectories.ReferenceTestSuit);
            handler.UnzipByteArray(submissionDto.ReferenceSolution, directory,
                workingDirectories.Solution);
            handler.UnzipByteArray(submissionDto.TestCaseSolution, directory,
                workingDirectories.StudentTestSuit);

            return true;
        }

        public bool ValidateSubmission(SubmissionDto submissionDto, out FeedbackDto feedback)
        {
            feedback = new FeedbackDto();
            if (submissionDto.TestCaseSolution == null)
            {
                feedback.Message = "Test Case Solution not provided";
                return false;
            }

            if (submissionDto.ReferenceTestSolution == null)
            {
                feedback.Message = "Reference Test Case Solution not provided";
                return false;
            }

            if (submissionDto.ReferenceSolution == null)
            {
                feedback.Message = "Reference Solution not provided";
                return false;
            }

            return true;
        }
    }
}
