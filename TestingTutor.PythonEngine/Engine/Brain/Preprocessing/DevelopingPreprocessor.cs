using System.IO;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Factory;
using TestingTutor.PythonEngine.Engine.Models;

namespace TestingTutor.PythonEngine.Engine.Brain.Preprocessing
{
    public class DevelopingPreprocessor : IPreprocessor
    {
        protected IEngineFactory Factory;

        public DevelopingPreprocessor(IEngineFactory factory)
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
            workingDirectories.Solution = Path.Combine(directory, Path.GetFileNameWithoutExtension(submissionDto.SolutionFolderName));
            workingDirectories.StudentTestSuit = Path.Combine(directory, "StudentTestSuit");

            handler.UnzipByteArray(submissionDto.ReferenceTestSolution, directory,
                workingDirectories.ReferenceTestSuit);
            handler.UnzipByteArray(submissionDto.AssignmentSolution, directory,
                workingDirectories.Solution);
            handler.UnzipByteArray(submissionDto.TestCaseSolution, directory,
                workingDirectories.StudentTestSuit);

            var exitCode = Factory.Pytest().Run(workingDirectories.StudentTestSuit, "$null", workingDirectories.StudentTestSuit);

            if (exitCode != 0)
            {
                feedback.Message = "Student's solution didn't pass their own Test Suite";
                return false;
            }

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

            if (submissionDto.AssignmentSolution == null)
            {
                feedback.Message = "Student's Solution not provided";
                return false;
            }

            return true;
        }
    }
}
