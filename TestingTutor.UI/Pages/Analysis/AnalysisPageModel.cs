using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.DataVisuals;
using Submission = TestingTutor.Dev.Data.Models.Submission;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Analysis
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class AnalysisPageModel : PageModel
    {
        public const string ClassAverage = "Class Average";
        public Random Random = new Random();


        protected ApplicationDbContext Context;

        public AnalysisPageModel(ApplicationDbContext context)
        {
            Context = context;
        }

        public Assignment Assignment { get; set; }

        public IList<string> Submitters { get; set; } = new List<string>();


        public IList<string> GetSubmitters(IList<Submission> submissions)
        {
            return submissions.Select(x => x.SubmitterId).Distinct().ToList();
        }
        

        public string FullName(ApplicationUser user)
        {
            return $"{user.FirstName} {user.LastName}";
        }
        

        public Charts AddClassAverage(Charts charts)
        {
            var classAverageColor = RandomColor();
            charts.Labels.Add(ClassAverage);
            charts.Colors.Add(classAverageColor);
            var lineCoverage = new List<int>();
            for (var i = 0; i < charts.Domain.Count; ++i)
            {
                var total = 0;
                var sum = 0;
                foreach (var line in charts.LineCoverageChart.Lines)
                {
                    if (i < line.Count)
                    {
                        total++;
                        sum += line[i];
                    }
                    else
                    {
                        total++;
                        sum += line.Last();
                    }
                }
                lineCoverage.Add((int)Math.Floor(sum * 1.0f / total));
            }
            charts.LineCoverageChart.Labels.Add(ClassAverage);
            charts.LineCoverageChart.Colors.Add(classAverageColor);
            charts.LineCoverageChart.Lines.Add(lineCoverage);

            var branchCoverage = new List<int>();
            for (var i = 0; i < charts.Domain.Count; ++i)
            {
                var total = 0;
                var sum = 0;
                foreach (var line in charts.BranchCoverageChart.Lines)
                {
                    if (i < line.Count)
                    {
                        total++;
                        sum += line[i];
                    }
                    else
                    {
                        total++;
                        sum += line.Last();
                    }
                }
                branchCoverage.Add((int)Math.Floor(sum * 1.0f / total));
            }
            charts.BranchCoverageChart.Labels.Add(ClassAverage);
            charts.BranchCoverageChart.Colors.Add(classAverageColor);
            charts.BranchCoverageChart.Lines.Add(branchCoverage);
            
            var conditionalCoverage = new List<int>();
            for (var i = 0; i < charts.Domain.Count; ++i)
            {
                var total = 0;
                var sum = 0;
                foreach (var line in charts.ConditionalCoverageChart.Lines)
                {
                    if (i < line.Count)
                    {
                        total++;
                        sum += line[i];
                    }
                    else
                    {
                        total++;
                        sum += line.Last();
                    }
                }
                conditionalCoverage.Add((int)Math.Floor(sum * 1.0f / total));
            }
            charts.ConditionalCoverageChart.Labels.Add(ClassAverage);
            charts.ConditionalCoverageChart.Colors.Add(classAverageColor);
            charts.ConditionalCoverageChart.Lines.Add(conditionalCoverage);

            var redundantTestCoverage = new List<int>();
            for (var i = 0; i < charts.Domain.Count; ++i)
            {
                var total = 0;
                var sum = 0;
                foreach (var line in charts.RedundantTestChart.Lines)
                {
                    if (i < line.Count)
                    {
                        total++;
                        sum += line[i];
                    }
                    else
                    {
                        total++;
                        sum += line.Last();
                    }
                }
                redundantTestCoverage.Add((int)Math.Floor(sum * 1.0f / total));
            }
            charts.RedundantTestChart.Labels.Add(ClassAverage);
            charts.RedundantTestChart.Colors.Add(classAverageColor);
            charts.RedundantTestChart.Lines.Add(redundantTestCoverage);

            return charts;
        }
        
        

        public FileStreamResult GetCsv(Charts charts)
        {
            var fileText = $"Line,";
            fileText += $"{charts.Domain.Select(d => $"{d}").Join(",")}\n";
            fileText += charts.LineCoverageChart.Lines.Select((l, i) => $"{charts.Labels[i]},{l.Select(num => $"{num}").Join(",")}").Join("\n");
            fileText += $"\nBranch,";
            fileText += $"{charts.Domain.Select(d => $"{d}").Join(",")}\n";
            fileText += charts.BranchCoverageChart.Lines.Select((l, i) => $"{charts.Labels[i]},{l.Select(num => $"{num}").Join(",")}").Join("\n");
            fileText += $"\nConditional,";
            fileText += $"{charts.Domain.Select(d => $"{d}").Join(",")}\n";
            fileText += charts.ConditionalCoverageChart.Lines.Select((l, i) => $"{charts.Labels[i]},{l.Select(num => $"{num}").Join(",")}").Join("\n");
            fileText += $"\nRedundant Tests,";
            fileText += $"{charts.Domain.Select(d => $"{d}").Join(",")}\n";
            fileText += charts.RedundantTestChart.Lines.Select((l, i) => $"{charts.Labels[i]},{l.Select(num => $"{num}").Join(",")}").Join("\n");

            var bytes = Encoding.ASCII.GetBytes(fileText);
            return new FileStreamResult(new MemoryStream(bytes), "text/csv")
            {
                FileDownloadName = "analysis.csv"
            };
        }

        private int CalculateConditionalCoveragePercentage(Submission submission)
        {
            var covered = submission.Feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.ConditionsCovered));
            var missed = submission.Feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.ConditionsMissed));
            if (covered + missed == 0) return 100;
            var percentage = (double)covered / (covered + missed);
            return (int)Math.Floor(percentage * 100.0f);
        }

        private int CalculateBranchCoveragePercentage(Submission submission)
        {
            var covered = submission.Feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.BranchesCovered));
            var missed = submission.Feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.BranchesMissed));
            if (covered + missed == 0) return 100;
            var percentage = (double)covered / (covered + missed);
            return (int)Math.Floor(percentage * 100.0f);
        }

        private int CalculateLineCoveragePercentage(Submission submission)
        {
            var covered = submission.Feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.LinesCovered));
            var missed = submission.Feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.LinesMissed));
            if (covered + missed == 0) return 100;
            var percentage = (double)covered / (covered + missed);
            return (int)Math.Floor(percentage * 100.0f);
        }


        public IList<Submission> GetSubmissions(IList<string> instructors)
        {
            return Context.Submissions
                .Where(s => instructors.All(i => !i.Equals(s.SubmitterId)))
                .Where(s => s.AssignmentId.Equals(Assignment.Id))
                .ToList()
                .Select(s =>
                {
                    Context.Entry(s).Reference(x => x.Feedback).Query()
                        .Include(x => x.EngineException).Load();
                    return s;
                })
                .Where(s => s.Feedback != null && s.Feedback.EngineException == null)
                .ToList()
                .Select(s =>
                {
                    Context.Entry(s).Reference(x => x.Feedback).Query()
                        .Include(x => x.ClassCoverages).ThenInclude(x => x.MethodCoverages)
                        .Include(x => x.InstructorTestResults).ThenInclude(x => x.StudentTestResults).Load();
                    return s;
                })
                .ToList();
        }
        

        public class Charts
        {
            public MultiLineChart LineCoverageChart { get; set; }
            public MultiLineChart BranchCoverageChart { get; set; }
            public MultiLineChart ConditionalCoverageChart { get; set; }
            public MultiLineChart RedundantTestChart { get; set; }
            public IList<string> Labels { get; set; }
            public IList<string> Colors { get; set; }
            public IList<int> Domain { get; set; }
        }

        public Charts CourseCharts { get; set; }

        public Charts GetCharts(IList<Submission> submissions)
        {
            var charts = new Charts();

            var groups = submissions.GroupBy(sub => sub.SubmitterId)
                .Select(g =>
                {
                    return new
                    {
                        Id = g.Key,
                        Feedback = g.ToList()
                    };
                })
                .ToList();

            charts.Labels = groups.Select(x => x.Id).ToList();
            charts.Colors = groups.Select(x => RandomColor()).ToList();

            var domain = new List<int>();
            var high = groups.Select(x => x.Feedback.Count).Max();
            for (var i = 1; i <= high; ++i) domain.Add(i);

            charts.Domain = domain;

            charts.LineCoverageChart = new MultiLineChart()
            {
                Id = "line_coverage",
                Colors = charts.Colors.ToList(),
                Labels = charts.Labels.ToList(),
                Minimum = 0,
                Maximum = 100,
                Domain = domain.ToList(),
                Lines = groups.Select(x =>
                {
                    IList<int> data = x.Feedback.Select(CalculateLineCoveragePercentage).ToList();
                    return data;
                }).ToList()
            };
            charts.BranchCoverageChart = new MultiLineChart()
            {
                Id = "branch_coverage",
                Colors = charts.Colors.ToList(),
                Labels = charts.Labels.ToList(),
                Minimum = 0,
                Maximum = 100,
                Domain = domain.ToList(),
                Lines = groups.Select(x =>
                {
                    IList<int> data = x.Feedback.Select(CalculateBranchCoveragePercentage).ToList();
                    return data;
                }).ToList()
            };
            charts.ConditionalCoverageChart = new MultiLineChart()
            {
                Id = "conditional_coverage",
                Colors = charts.Colors.ToList(),
                Labels = charts.Labels.ToList(),
                Minimum = 0,
                Maximum = 100,
                Domain = domain.ToList(),
                Lines = groups.Select(x =>
                {
                    IList<int> data = x.Feedback.Select(CalculateConditionalCoveragePercentage).ToList();
                    return data;
                }).ToList()
            };

            charts.RedundantTestChart = new MultiLineChart()
            {
                Id = "redundant_tests",
                Colors = charts.Colors.ToList(),
                Labels = charts.Labels.ToList(),
                Minimum = 0,
                Domain = domain.ToList(),
                Lines = groups.Select(x =>
                {
                    IList<int> data = x.Feedback.Select(CalculateRedundantTest).ToList();
                    return data;
                }).ToList()
            };
            charts.RedundantTestChart.Maximum = charts.RedundantTestChart.Lines.SelectMany(x => x).Max();

            return charts;
        }


        public int CalculateRedundantTest(Submission submission)
        {
            var set = new HashSet<string>();
            submission.Feedback.InstructorTestResults.ToList().ForEach(
                i =>
                {
                    i.StudentTestResults.Skip(1).ToList().ForEach(
                        s => set.Add(s.TestName)
                    );
                }
            );
            return set.Count;
        }


        public string RandomColor()
        {
            return "#" + Random.Next(255).ToString("X2") + Random.Next(255).ToString("X2") + Random.Next(255).ToString("X2");
        }

    }
}
