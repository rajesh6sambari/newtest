using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.EngineModels;
using System.Threading;
using TestingTutor.CSharpEngine.Engine.Feedback;
using TestingTutor.CSharpEngine.Engine.Factory;

namespace TestingTutor.CSharpEngine.Engine
{
    public class CSharpEngine : IEngine
    {
        protected IEngineFactory Factory;

        public CSharpEngine(IEngineFactory factory)
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

                if (preprocess.Preprocessing(submissionDto, path, out var workingDirectories, out var feedback))
                {
                    var analysis = Factory.TestCaseAnalysis();

                    analysis.Run(submissionDto, ref workingDirectories, ref feedback);

                    var coverage = Factory.CoverageStats();
                }

                feedback.StudentId = submissionDto.SubmitterId;
                feedback.SubmissionId = submissionDto.SubmissionId;

                //var sender = Factory.FeedbackSender();
                //Commented out for tests
                //sender.SendFeedback(feedback);

                return Task.FromResult(feedback);
            }

        }
    }
}
