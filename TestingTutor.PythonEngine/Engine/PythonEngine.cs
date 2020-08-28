using System;
using System.Threading;
using System.Threading.Tasks;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Factory;

namespace TestingTutor.PythonEngine.Engine
{
    public class PythonEngine : IEngine
    {
        protected IEngineFactory Factory;

        public PythonEngine(IEngineFactory factory)
        {
            Factory = factory;
        }

        public Task<FeedbackDto> Run(SubmissionDto submissionDto)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            using (var workspace = Factory.Workspace(Convert.ToString(id)))
            {
                var path = workspace.CreateDirectory();

                var preprocess = Factory.Preprocessor(submissionDto.ApplicationMode);

                if (preprocess.Preprocessing(submissionDto, path, out var workingDirectories, out var feedbackDto))
                {
                    var analysis = Factory.TestCaseAnalysis();

                    analysis.Run(submissionDto, ref workingDirectories, ref feedbackDto);

                    var coverage = Factory.CoverageStats();

                    coverage.GetStats(submissionDto, ref workingDirectories, ref feedbackDto);
                }

                feedbackDto.StudentId = submissionDto.SubmitterId;
                feedbackDto.SubmissionId = submissionDto.SubmissionId;

                return Task.FromResult(feedbackDto);
            }
        }
    }
}
