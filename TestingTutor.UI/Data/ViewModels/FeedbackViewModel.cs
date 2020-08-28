using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Data.ViewModels
{
    public enum TestStatus
    {
        Covered,
        Uncovered,
        Redundant,
        Failed
    }

    public class TopLevelPageFeedback
    {
        public bool ShouldDisplay { get; set; }
        public string Message { get; set; }
    }

    public class TestSuiteFeedback
    {
        public bool ShouldDisplay { get; set; }

        [Display(Name = "Coverage Percentage")]
        public double CoveragePercentage { get; set; }

        [Display(Name = "Number of Branches Hit")]
        public int NumberOfBranchesHit { get; set; }

        [Display(Name = "Number of Statements")]
        public int NumberOfStatements { get; set; }

        [Display(Name = "Number of Missing Statements")]
        public int NumberOfMissingStatements { get; set; }
    }

    public class DetailedFeedback
    {
        public bool ShouldDisplay { get; set; }
        public IList<TestCaseResult> TestCaseResults { get; set; }
    }

    public class TestConceptDetails
    {
        public string Conceptual { get; set; }
        public string Detailed { get; set; }
        public string Anchor { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TestConceptDetails details &&
                   (details.Conceptual.Equals(Conceptual) && details.Detailed.Equals(Detailed));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Conceptual, Detailed);
        }
    }

    public class ConceptualFeedback
    {
        public bool ShouldDisplay { get; set; }
        public IList<TestConceptDetails> Feedback { get; set; }
    }

    public class StudentTestResult
    {
        [Display(Name = "Test Case Name")] public string Name { get; set; }
        [Display(Name = "Pass/Fail")] public bool Passed { get; set; }
    }

    public class TestCaseResult
    {
        [Display(Name = "Equivalence Class")] public string Name { get; set; }
        [Display(Name = "Redundant Test Cases Written")] public bool HasRedundantTestCases => StudentTestResults.Count > 1;
        [Display(Name = "Test Status")] public TestStatus TestStatus { get; set; }
        [Display(Name = "Student Test Results")] public IList<StudentTestResult> StudentTestResults { get; set; }
        [Display(Name = "Concepts Checked")] public IList<TestConceptDetails> Concepts { get; set; }
    }

    public class RawCoverageFeedback
    {
        public IList<ClassCoverage> ClassCoverages { get; set; } = new List<ClassCoverage>();
    }

    public class FeedbackViewModel
    {
        private readonly ApplicationDbContext _context;
        private readonly Assignment _assignment;
        private readonly Submission _submission;
        private readonly Feedback _feedback;

        public FeedbackViewModel(ApplicationDbContext applicationDbContext, Submission submission)
        {
            _context = applicationDbContext;
            _assignment = submission.Assignment;
            _submission = submission;
            _feedback = submission.Feedback;

            if (_feedback != null)
            {
                GenerateFeedback();
            }

        }

        public TopLevelPageFeedback TopLevelPageFeedback { get; private set; }
        public DetailedFeedback DetailedFeedback { get; private set; }
        public ConceptualFeedback ConceptualFeedback { get; private set; }
        public RawCoverageFeedback RawCoverageFeedback { get; private set; }

        private void GenerateFeedback()
        {
            GenerateTopLevelPageFeedback();
            GenerateRawCoverageFeedback();
            GenerateDetailedFeedback();
            GenerateConceptualFeedback();
        }

        private void GenerateRawCoverageFeedback()
        {
            RawCoverageFeedback = new RawCoverageFeedback()
            {
                ClassCoverages = _feedback.ClassCoverages
                    .OrderBy(s => s.Name)
                    .ToList(),
            };
        }

        private void GenerateTopLevelPageFeedback()
        {
            TopLevelPageFeedback = new TopLevelPageFeedback
            {
                ShouldDisplay = _assignment.FeedbackLevelOption.Name.Equals("Detailed Feedback") || _assignment.FeedbackLevelOption.Name.Equals("Conceptual Feedback"),
                Message = _feedback.EngineException == null ? "" :
                    $"{_feedback.EngineException.Phase}\nFrom - {_feedback.EngineException.From}\nReport -\n{_feedback.EngineException.Report}\n"
            };
        }

        private void GenerateDetailedFeedback()
        {
            DetailedFeedback = new DetailedFeedback
            {
                ShouldDisplay = _assignment.FeedbackLevelOption.Name.Equals("Detailed Feedback"),
                TestCaseResults = new List<TestCaseResult>()
            };

            foreach (var result in _feedback.InstructorTestResults)
            {
                var newTestResult = new TestCaseResult
                {
                    Name = result.EquivalenceClass,
                    TestStatus = result.TestCaseStatus.Name.Equals("Covered")
                        ? TestStatus.Covered
                        : TestStatus.Uncovered,
                    Concepts = new List<TestConceptDetails>(),
                    StudentTestResults = new List<StudentTestResult>()
                };

                result.StudentTestResults.ToList().ForEach(r => newTestResult.StudentTestResults.Add(
                    new StudentTestResult
                    {
                        Name = r.TestName,
                        Passed = r.Pass
                    }));

                result.TestResultConcepts.ToList().ForEach(r =>
                {
                    var concept = _context.TestConcepts.Single(c => c.Id.Equals(r.TestConceptId));

                    newTestResult.Concepts.Add(new TestConceptDetails { Name = concept.Name, Anchor = "a" + concept.Id, Conceptual = concept.ConceptualContent, Detailed = concept.DetailedContent });
                });

                if (newTestResult.HasRedundantTestCases)
                {
                    newTestResult.TestStatus = TestStatus.Redundant;
                }

                DetailedFeedback.TestCaseResults.Add(newTestResult);
            }
        }

        private void GenerateConceptualFeedback()
        {
            ConceptualFeedback = new ConceptualFeedback()
            {
                Feedback = new List<TestConceptDetails>(),
                ShouldDisplay = _assignment.FeedbackLevelOption.Name.Equals("Conceptual Feedback"),
            };

            _feedback.InstructorTestResults.ToList().ForEach(r =>
            {
                r.TestResultConcepts.ToList().ForEach(c =>
                {
                    var testConcept = _context.TestConcepts.Single(concept => concept.Id.Equals(c.TestConceptId));
                    ConceptualFeedback.Feedback.Add(new TestConceptDetails { Anchor = "a" + testConcept.Id, Conceptual = testConcept.ConceptualContent, Detailed = testConcept.DetailedContent });
                });
            });

            ConceptualFeedback.Feedback = ConceptualFeedback.Feedback.Distinct().ToList();
        }
    }
}
