using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.DataVisuals;

namespace TestingTutor.UI.Data.ViewModels
{
    // TODO: Student deposit shouldn't show for one to one mapping

    public class AssignmentFeedbackViewModel
    {
        public IList<Submission> Submissions { get; set; }
        public int Index { get; set; }
        public Submission Submission => Submissions[Index];
        public Assignment Assignment => Submission.Assignment;

        public string FeedbackLevel => Assignment.FeedbackLevelOption.Name;
        public double TestCoverageLevel => Assignment.TestCoverageLevel;
        public double RedundantTestLevel => Assignment.RedundantTestLevel;

        public int SubmissionTestCoveragePercentage(int index)
        {
            if (Submissions[index].Feedback.InstructorTestResults.Count == 0)
                return 100;
            var instructorTestResults = new List<InstructorTestResult>();
            var alreadyStudentTestNames = new HashSet<string>();
            foreach (var test in Submissions[index].Feedback.InstructorTestResults)
            {
                // Test isn't covered
                if (test.StudentTestResults.Count == 0)
                    continue;
                // Looking for an instructor test where
                // all of its student test aren't already covered
                // by passed student test
                if (test.StudentTestResults.All(s => !alreadyStudentTestNames.Contains(s.TestName)))
                {
                    instructorTestResults.Add(test);
                    test.StudentTestResults.ToList().ForEach(s => alreadyStudentTestNames.Add(s.TestName));
                }
            }

            return (int) Math.Floor((double) instructorTestResults.Count /
                                    Submissions[index].Feedback.InstructorTestResults.Count * 100);
        }

        public bool LineCoverage =>
            Assignment.AssignmentCoverageTypeOptions.FirstOrDefault(c =>
                c.IsChecked && c.CoverageTypeOption.Name.Equals("Statement")) != null;

        public bool BranchCoverage =>
            Assignment.AssignmentCoverageTypeOptions.FirstOrDefault(c =>
                c.IsChecked && c.CoverageTypeOption.Name.Equals("Branch")) != null;

        public bool ConditionCoverage =>
            Assignment.AssignmentCoverageTypeOptions.FirstOrDefault(c =>
                c.IsChecked && c.CoverageTypeOption.Name.Equals("Condition")) != null;

        public ThresholdMultiLineChart GetCoverageChart()
        {
            var chart = new ThresholdMultiLineChart()
            {
                Id = "coverage_chart",
                Minimum = new ChartPoint()
                {
                    X = 1.0f,
                    Y = 0.0f,
                },
                Maximum = new ChartPoint()
                {
                    X = Submissions.Count,
                    Y = 100.0f,
                },
                XAxis = "Submission #",
                YAxis = "Coverage %",
                ThresholdLine = new ThresholdMultiLineChartLine()
                {
                    Color = "#28a745",
                    Label = "Goal",
                    Points = Enumerable.Repeat(0, Submissions.Count)
                        .Select((_, x) => new ChartPoint()
                        {
                            X = x + 1,
                            Y = TestCoverageLevel
                        }).ToList()
                },
                MarkColor = "#dc3545",
                XMark = Index + 1
            };

            if (LineCoverage)
                chart.Lines.Add(GetThresholdMultiLineChartLine("Line Coverage", "#007bff",
                    SubmissionLineCoverageProgress));

            if (BranchCoverage)
                chart.Lines.Add(GetThresholdMultiLineChartLine("Branch Coverage", "#6f42c1",
                    SubmissionBranchCoverageProgress));

            if (ConditionCoverage)
                chart.Lines.Add(GetThresholdMultiLineChartLine("Condition Coverage", "#17a2b8",
                    SubmissionConditionCoverageProgress));

            return chart;
        }

        public ThresholdMultiLineChart GetRedundantChart()
        {
            return new ThresholdMultiLineChart()
            {
                Id = "redundant_chart",
                XAxis = "Submission #",
                YAxis = "# Redundant Tests",
                Minimum = new ChartPoint()
                {
                    X = 1,
                    Y = 0,
                },
                Maximum = new ChartPoint()
                {
                    X = Submissions.Count,
                    Y = 100,
                }, 
                Lines = new List<ThresholdMultiLineChartLine>()
                {
                    new ThresholdMultiLineChartLine()
                    {
                        Label = "Redundant Tests Line",
                        Color = "#007bff",
                        Points = Submissions.Select((s, x) => new ChartPoint()
                        {
                            X = x + 1,
                            Y = CalculateSubmissionRedundancyPercentage(s),
                        }).ToList(),
                    }
                },
                ThresholdLine = new ThresholdMultiLineChartLine()
                {
                    Color = "#28a745",
                    Label = "Max",
                    Points = Enumerable.Repeat(0, Submissions.Count)
                        .Select((_, x) => new ChartPoint()
                        {
                            X = x + 1,
                            Y = RedundantTestLevel
                        }).ToList()
                },
                MarkColor = "#dc3545",
                XMark = Index + 1
            };
        }

        private double CalculateSubmissionRedundancyPercentage(Submission submission)
        {
            var total = submission.Feedback
                .InstructorTestResults
                .SelectMany(x => x.StudentTestResults)
                .Distinct((x, y) => x.TestName == y.TestName).Count();
            if (total == 0) return 0.0f;

            var set = new HashSet<string>();
            submission.Feedback.InstructorTestResults.ToList().ForEach(
                i =>
                {
                    i.StudentTestResults.Skip(1).ToList().ForEach(
                        s => set.Add(s.TestName)
                    );
                }
            );
            return Math.Truncate((double) set.Count / total * 100.0f);
        }

        private ThresholdMultiLineChartLine GetThresholdMultiLineChartLine(string label, string color,
            Func<Submission, double> progressFunction)
        {
            return new ThresholdMultiLineChartLine()
            {
                Label = label,
                Color = color,
                Points = Submissions.Select((s, x) => 
                    (s.Feedback == null || s.Feedback.EngineException != null)
                    ? new ChartPoint()
                    {
                        X = x + 1,
                        Y = 0,
                    } :
                    new ChartPoint()
                    {
                        X = x + 1,
                        Y = progressFunction(s)
                    }).ToList()
            };
        }

        private double SubmissionLineCoverageProgress(Submission s)
        {
            var covered = s.Feedback.ClassCoverages.SelectMany(x => x.MethodCoverages).Sum(x => x.LinesCovered);
            var missed = s.Feedback.ClassCoverages.SelectMany(x => x.MethodCoverages).Sum(x => x.LinesMissed);
            if (missed + covered == 0) return 0;
            return Math.Truncate((double)covered / (covered + missed) * 100.0f);
        }

        private double SubmissionBranchCoverageProgress(Submission s)
        {
            var covered = s.Feedback.ClassCoverages.SelectMany(x => x.MethodCoverages).Sum(x => x.BranchesCovered);
            var missed = s.Feedback.ClassCoverages.SelectMany(x => x.MethodCoverages).Sum(x => x.BranchesMissed);
            if (missed + covered == 0) return 0;
            return Math.Truncate((double)covered / (covered + missed) * 100.0f);
        }

        private double SubmissionConditionCoverageProgress(Submission s)
        {
            var covered = s.Feedback.ClassCoverages.SelectMany(x => x.MethodCoverages).Sum(x => x.ConditionsCovered);
            var missed = s.Feedback.ClassCoverages.SelectMany(x => x.MethodCoverages).Sum(x => x.ConditionsMissed);
            if (missed + covered == 0) return 0;
            return Math.Truncate((double)covered / (covered + missed) * 100.0f);
        }

        public IList<InstructorTestResult> UncoveredInstructorTestResults()
        {
            var uncovered = new List<InstructorTestResult>();
            foreach (var test in Submission.Feedback.InstructorTestResults)
            {
                // No test covered the instructor
                if (test.StudentTestResults.Count == 0)
                {
                    uncovered.Add(test);
                }
                else
                {
                    // This if statment checks if the instructor test has 
                    // any student test which only covereds this particular
                    // instructor test. The negation infront is if it doesn't
                    // have a unquie student test then it's conside 
                    // uncovered because we don't know if its student test
                    // covered particular conditionals or not. 
                    if (!test.StudentTestResults.Any(student =>
                    {
                        foreach (var instructor in Submission.Feedback.InstructorTestResults.Where(x => x.Id != test.Id)
                        )
                        {
                            if (instructor.StudentTestResults.Any(x => x.TestName.Equals(student.TestName)))
                            {
                                return false;
                            }
                        }

                        return true;
                    }))
                    {
                        uncovered.Add(test);
                    }
                }
            }
            return uncovered;
        }

        public IList<InstructorTestResult> CoveredInstructorTestResults()
        {
            var uncovered = UncoveredInstructorTestResults();
            return Submission.Feedback
                .InstructorTestResults
                .Where(x => !uncovered.Any(y => y.EquivalenceClass.Split(';')[0].Equals(x.EquivalenceClass.Split(';')[0])))
                .ToList();
        }
    }
}
