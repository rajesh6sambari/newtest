using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.DataVisuals;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Analysis
{
    public class StudentModel : AnalysisPageModel
    {
        public StudentModel(ApplicationDbContext context) : base(context)
        {
        }

        public string SubmitterId { get; set; }

        public IList<Submission> Submissions { get; set; }

        public MultiRadarChart RadarChart;
        public BarChart BarChart;

        public async Task<IActionResult> OnGetAsync(int? id, string submitter)
        {
            Assignment = await Context.GetAssignmentById(id);
            if (Assignment == null) return NotFound();
            var instructors = Assignment.Instructors.ToList().Select(a => a.Instructor.Id).ToList();

            Submissions = GetSubmissions(instructors).ToList();

            if (!Submissions.Any())
                return NotFound();

            SubmitterId = submitter;

            CourseCharts = GetCharts(Submissions);
            CourseCharts = AddClassAverage(CourseCharts);
            CourseCharts = KeepStudentAndClassAverage(CourseCharts, submitter);

            CreateRadarCharts();
            CreateBarChart();
            Submissions = Submissions.Where(x => x.SubmitterId.Equals(submitter)).ToList();

            return Page();
        }

        public void CreateBarChart()
        {
            BarChart = new BarChart()
            {
                Id = "redundant_bar_chart",
                Colors = CourseCharts.Colors.ToList(),
                Minimum = 0,
                Labels = CourseCharts.Labels.ToList(),
                Values = CourseCharts.RedundantTestChart.Lines.Select(l => (double)l.Last()).ToList(),
            };
        }

        public void CreateRadarCharts()
        {
            RadarChart = new MultiRadarChart()
            {
                Id = "latest_radar_chart",
                Colors = CourseCharts.Colors.ToList(),
                Minimum = 0,
                Maximum = 100,
                MultiRadarChartAxises = new List<MultiRadarChartAxis>()
                {
                    new MultiRadarChartAxis()
                    {
                        Axis = "Line",
                        Values = CourseCharts.LineCoverageChart.Lines.Select(l => (double)l.Last()).ToList()
                    },
                    new MultiRadarChartAxis()
                    {
                        Axis = "Branch",
                        Values = CourseCharts.BranchCoverageChart.Lines.Select(l => (double)l.Last()).ToList()
                    },
                    new MultiRadarChartAxis()
                    {
                        Axis = "Conditional",
                        Values = CourseCharts.ConditionalCoverageChart.Lines.Select(l => (double)l.Last()).ToList()
                    }
                }
            };
        }

        public async Task<FileStreamResult> OnPostAsync(int id, string type)
        {
            var submission = await Context.Submissions.FindAsync(id);

            switch (type)
            {
                case "Solution":
                    return new FileStreamResult(new MemoryStream(submission.SubmitterSolution), "application/zip");
                case "TestSolution":
                    return new FileStreamResult(new MemoryStream(submission.SubmitterTestCaseSolution), "application/zip");
            }
            return null;
        }

        public Charts KeepStudentAndClassAverage(Charts courseCharts, string submitterName)
        {
            var studentIndex = courseCharts.Labels.IndexOf(submitterName);
            var classIndex = courseCharts.Labels.IndexOf(ClassAverage);

            var charts = new Charts()
            {
                Labels = new List<string>() { submitterName, ClassAverage },
                Colors = new List<string>() { RandomColor(), RandomColor() },
                Domain = courseCharts.Domain,
            };

            var lineCoverageCharts = new MultiLineChart()
            {
                Labels = charts.Labels.ToList(),
                Colors = charts.Colors.ToList(),
                Domain = charts.Domain.ToList(),
                Id = courseCharts.LineCoverageChart.Id,
                Minimum = 0,
                Maximum = 100,
                Lines = new List<IList<int>>()
                {
                    courseCharts.LineCoverageChart.Lines[studentIndex],
                    courseCharts.LineCoverageChart.Lines[classIndex]
                }
            };
            charts.LineCoverageChart = lineCoverageCharts;

            var branchCoverageCharts = new MultiLineChart()
            {
                Labels = charts.Labels.ToList(),
                Colors = charts.Colors.ToList(),
                Domain = charts.Domain.ToList(),
                Id = courseCharts.BranchCoverageChart.Id,
                Minimum = 0,
                Maximum = 100,
                Lines = new List<IList<int>>()
                {
                    courseCharts.BranchCoverageChart.Lines[studentIndex],
                    courseCharts.BranchCoverageChart.Lines[classIndex]
                }
            };
            charts.BranchCoverageChart = branchCoverageCharts;

            var conditionalLineChart = new MultiLineChart()
            {
                Labels = charts.Labels,
                Colors = charts.Colors,
                Domain = charts.Domain,
                Id = courseCharts.ConditionalCoverageChart.Id,
                Minimum = 0,
                Maximum = 100,
                Lines = new List<IList<int>>()
                {
                    courseCharts.ConditionalCoverageChart.Lines[studentIndex],
                    courseCharts.ConditionalCoverageChart.Lines[classIndex]
                }
            };
            charts.ConditionalCoverageChart = conditionalLineChart;

            var redundantTestChart = new MultiLineChart()
            {
                Labels = charts.Labels,
                Colors = charts.Colors,
                Domain = charts.Domain,
                Id = courseCharts.RedundantTestChart.Id,
                Minimum = 0,
                Maximum = courseCharts.RedundantTestChart.Maximum,
                Lines = new List<IList<int>>()
                {
                    courseCharts.RedundantTestChart.Lines[studentIndex],
                    courseCharts.RedundantTestChart.Lines[classIndex]
                }
            };
            charts.RedundantTestChart = redundantTestChart;

            return charts;
        }

        [HttpPost, ActionName("Download")]
        public async Task<FileStreamResult> OnPostDownload(int? id, string submitter)
        {
            Assignment = await Context.GetAssignmentById(id);

            if (Assignment == null) return null;
            var instructors = Assignment.Instructors.ToList().Select(a => a.Instructor.Id).ToList();

            var submissions = GetSubmissions(instructors).ToList();
            SubmitterId = submitter;

            CourseCharts = GetCharts(submissions);
            CourseCharts = AddClassAverage(CourseCharts);
            CourseCharts = KeepStudentAndClassAverage(CourseCharts, submitter);

            return GetCsv(CourseCharts);
        }
    }
}