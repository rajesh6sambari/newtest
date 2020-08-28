using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestingTutor.EngineModels;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Controllers
{
    [Route("api/Feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Feedback
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostFeedback([FromBody] FeedbackDto feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var feedback = MapFeedbackDtoToFeedbackModel(feedbackDto);

                var submission = _context.Submissions.Single(s => s.Id.Equals(feedbackDto.SubmissionId));
                submission.Feedback = feedback;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

            return Ok();
        }

        private Feedback MapFeedbackDtoToFeedbackModel(FeedbackDto feedbackDto)
        {
            var feedback = new Feedback
            {
                EngineException= feedbackDto.EngineExceptionDto != null ? GetEngineException(feedbackDto.EngineExceptionDto) : null,
                // TODO: Class Coverage
                InstructorTestResults = GetInstructorTestResults(feedbackDto.InstructorTests).ToList()
            };

            return feedback;
        }

        private EngineException GetEngineException(EngineExceptionDto engineExceptionDto)
        {
            return new EngineException()
            {
                Report = engineExceptionDto?.Report,
                From = engineExceptionDto?.From,
                Phase = engineExceptionDto?.Phase,
            };
        }

        private IEnumerable<StudentTestResult> GetStudentTestResults(IEnumerable<StudentTestDto> testResultsDto)
        {

            var testResults = new List<StudentTestResult>();

            testResultsDto.ToList()
                .ForEach(tr => testResults.Add(new StudentTestResult()
                {
                    TestName = tr.Name,
                    Pass = tr.Passed,
                    TestCaseStatus =
                        _context.TestCaseStatuses.Single(tcs => tcs.Name.Equals(tr.TestStatus.ToString()))
                }));

            return testResults;
        }

        private IEnumerable<InstructorTestResult> GetInstructorTestResults(IEnumerable<InstructorTestDto> testResultsDto)
        {
            var testResults = new List<InstructorTestResult>();

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
                        TestCaseStatus =
                            _context.TestCaseStatuses.Single(tcs => tcs.Name.Equals(tr.TestStatus.ToString()))
                    };

                    result.TestResultConcepts = GetTestConcepts(concepts, result).ToList();
                    testResults.Add(result);
                });

            return testResults;
        }

        private IEnumerable<TestResultConcept> GetTestConcepts(IEnumerable<string> concepts, InstructorTestResult instructorTestResult)
        {
            foreach (var concept in concepts)
            {
                var testConcept = _context.TestConcepts.SingleOrDefault(c => c.Name.ToUpper().Equals(concept.ToUpper()));
                if (testConcept != null) yield return new TestResultConcept {TestConceptId = testConcept.Id, InstructorTestResult = instructorTestResult};
            }
        }
    }
}