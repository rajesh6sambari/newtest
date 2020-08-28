using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestingTutor.EngineModels;
using TestingTutor.UI.Data;

using TestingTutor.UI.Hubs;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Utilities
{
    public class EngineSubmitter : JobActivator
    {
        //private readonly IAppDbContextFactory _appDbContextFactory;
        private readonly IServiceProvider _serviceProvider;
        public EngineSubmitter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            _serviceProvider.CreateScope();
            return _serviceProvider.GetService(jobType);
        }

        public ApplicationDbContext CreateDatabase()
        {
            return (ApplicationDbContext)ActivateJob(typeof(ApplicationDbContext));
        }

        public IHubContext<SubmissionHub> CreateHub()
        {
            return _serviceProvider.GetService<IHubContext<SubmissionHub>>();
            //return (IHubContext<SubmissionHub>) ActivateJob(typeof(IHubContext<SubmissionHub>));
        }

        [AutomaticRetry(Attempts = 1)]
        public void Submit(JavaEngine.Engine.JavaEngine engine, SubmissionDto submissionDto, string username)
        {
            try
            {
                var feedbackDtoResult = engine.Run(submissionDto);//BackgroundJob.Enqueue(() => _engine.Run(submissionDto));
                var feedback = MapFeedbackDtoToFeedbackModel(feedbackDtoResult.Result);

                int assignmentId;

                using (var context = CreateDatabase())
                {
                    context.Feedback.Add(feedback);

                    context.SaveChanges();

                    var submission = context.Submissions.Single(s => s.Id.Equals(submissionDto.SubmissionId));
                    submission.FeedbackId = feedback.Id;
                    context.SaveChanges();

                    assignmentId = submission.AssignmentId;
                }

                var hub = CreateHub();

                if (SubmissionHub.Users.TryGetValue(username, out var user))
                {
                    IList<string> cids;
                    lock (user.ConnectionIds)
                    {
                        cids = user.ConnectionIds.ToList();
                    }

                    int submissionIndex;
                    using (var context = CreateDatabase())
                    {
                        submissionIndex = context.Submissions.AsNoTracking()
                            .Count(s => s.SubmitterId.Equals(submissionDto.SubmitterId) && s.AssignmentId == assignmentId) - 1;
                    }

                    foreach (var cid in cids)
                    {
                        hub.Clients.Client(cid).SendAsync("FeedbackFinish", assignmentId, submissionIndex).Wait();
                    }

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }


        private Feedback MapFeedbackDtoToFeedbackModel(FeedbackDto feedbackDto)
        {
            var feedback = new Feedback
            {
                EngineException = feedbackDto.EngineExceptionDto != null ? GetEngineException(feedbackDto.EngineExceptionDto) : null,
                ClassCoverages = GetClassCoverages(feedbackDto.ClassCoveragesDto),
                InstructorTestResults = GetInstructorTestResults(feedbackDto.InstructorTests).ToList()
            };

            return feedback;
        }

        private ICollection<ClassCoverage> GetClassCoverages(IList<ClassCoverageDto> classCoverageDtos)
        {
            return classCoverageDtos
                .Select(classCoverageDto => new ClassCoverage()
                {
                    Container = classCoverageDto.Container,
                    Name = classCoverageDto.Name,
                    MethodCoverages = GetMethodCoverages(classCoverageDto.MethodCoveragesDto),
                })
                .ToList();
        }

        private IList<MethodCoverage> GetMethodCoverages(IList<MethodCoverageDto> methodCoveragesDto)
        {
            return methodCoveragesDto.Select(methodDto => new MethodCoverage()
            {
                Name = methodDto.Name,
                BranchesCovered = methodDto.BranchesCovered,
                BranchesMissed = methodDto.BranchesMissed,
                ConditionsCovered = methodDto.ConditionsCovered,
                ConditionsMissed = methodDto.ConditionsMissed,
                LinesCovered = methodDto.LinesCovered,
                LinesMissed = methodDto.LinesMissed
            })
                .ToList();
        }

        private EngineException GetEngineException(EngineExceptionDto engineExceptionDto)
        {
            return new EngineException()
            {
                Report = engineExceptionDto?.Report,
                From = engineExceptionDto?.From,
                Phase = engineExceptionDto?.Phase
            };
        }

        private IEnumerable<StudentTestResult> GetStudentTestResults(IEnumerable<StudentTestDto> testResultsDto)
        {
            var testResults = new List<StudentTestResult>();

            using (var context = CreateDatabase())
            {
                var testCaseStatus = context.TestCaseStatuses;
                testResultsDto.ToList()
                    .ForEach(tr => testResults.Add(new StudentTestResult()
                    {
                        TestName = tr.Name,
                        Pass = tr.Passed,
                        TestCaseStatusId =
                            testCaseStatus.Single(tcs => tcs.Name.Equals(tr.TestStatus.ToString())).Id
                    }));
            }

            return testResults;

        }

        private IEnumerable<InstructorTestResult> GetInstructorTestResults(IEnumerable<InstructorTestDto> testResultsDto)
        {
            var testResults = new List<InstructorTestResult>();

            using (var context = CreateDatabase())
            {
                var testCaseStatuses = context.TestCaseStatuses;
                testResultsDto.ToList()
                    .ForEach(tr =>
                    {
                        var concepts = new List<string>(tr.Concepts);
                        if (tr.StudentTests.Count > 1)
                        {
                            concepts.Clear();
                            concepts.Add("REDUNDANT_TEST");
                        }

                        var result = new InstructorTestResult()
                        {
                            TestName = tr.Name,
                            EquivalenceClass = tr.EquivalenceClass,
                            StudentTestResults = GetStudentTestResults(tr.StudentTests).ToList(),
                            TestCaseStatusId =
                               testCaseStatuses.Single(tcs => tcs.Name.Equals(tr.TestStatus.ToString())).Id,
                            Passed = tr.Passed,
                        };

                        result.TestResultConcepts = GetTestConcepts(concepts, result).ToList();
                        testResults.Add(result);
                    });
            }

            return testResults;
        }

        private IEnumerable<TestResultConcept> GetTestConcepts(IEnumerable<string> concepts, InstructorTestResult instructorTestResult)
        {
            using (var context = CreateDatabase())
            {
                var testConcepts = context.TestConcepts.ToList();
                foreach (var concept in concepts)
                {
                    var testConcept =
                        testConcepts.SingleOrDefault(c => c.Name.ToUpper().Equals(concept.ToUpper()));
                    if (testConcept != null)
                        yield return new TestResultConcept
                        { TestConceptId = testConcept.Id, InstructorTestResult = instructorTestResult };
                }
            }
        }
    }
}

