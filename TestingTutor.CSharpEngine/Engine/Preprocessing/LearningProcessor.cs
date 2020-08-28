using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.CSharpEngine.Models;
using TestingTutor.CSharpEngine.Engine;
using TestingTutor.CSharpEngine.Engine.Preprocessing;
using TestingTutor.EngineModels;
using System.IO;
using TestingTutor.CSharpEngine.Engine.Factory;


namespace TestingTutor.CSharpEngine.Engine.Preprocessing
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

            handler.UnzipArray(submissionDto.ReferenceTestSolution, directory,
                workingDirectories.ReferenceTestSuit);
            handler.UnzipArray(submissionDto.ReferenceSolution, directory,
                workingDirectories.Solution);
            handler.UnzipArray(submissionDto.TestCaseSolution, directory,
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
