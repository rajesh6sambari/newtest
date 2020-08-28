using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.DataVisuals;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Analysis
{
    public class MultiAnalysisModel : PageModel
    {
        protected ApplicationDbContext Context;
        public static Random Random = new Random();

        public MultiAnalysisModel(ApplicationDbContext context)
        {
            Context = context;
        }

        public string Selected { get; set; }

        public IList<string> Instructors { get; set; }

        public IList<Assignment> Assignments { get; set; } = new List<Assignment>();

        public GroupBarChart Chart { get; set; }

        public async Task<IActionResult> OnGetAsync(string selected)
        {
            Selected = selected;
            var indices = selected.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (selected.Length < 2) return NotFound();

            var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;

            var assignments = (await Context.GetAssignmentsAsync()).Where(a => a.InstitutionId.Equals(institutionalId)).ToList();
            foreach (var index in indices)
            {
                if (index >= 0 && index < assignments.Count)
                {
                    Assignments.Add(assignments[index]);
                }
            }

            if (assignments.Count < 2) return NotFound();

            Instructors = Assignments.First().Instructors.ToList().Select(a => a.Instructor.Id).ToList();

            Chart = GetChart();

            return Page();
        }

        [HttpPost, ActionName("Download")]
        public async Task<FileStreamResult> OnPostDownloadAsync(string selected)
        {
            var indices = selected.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (selected.Length < 2) return null;

            var institutionalId = Context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;

            var assignments = (await Context.GetAssignmentsAsync()).Where(a => a.InstitutionId.Equals(institutionalId)).ToList();
            foreach (var index in indices)
            {
                if (index >= 0 && index < assignments.Count)
                {
                    Assignments.Add(assignments[index]);
                }
            }

            if (assignments.Count < 2) return null;
            Instructors = Assignments.First().Instructors.ToList().Select(a => a.Instructor.Id).ToList();


            var fileText = $"Assignment,Course,Alias Id,Line,Branch,Conditional,Redundant,# of Valid Feedback\n";

            Assignments.ToList().ForEach(asg =>
            {
                var submissions = GetSubmissions(Instructors, asg);
                var studentAverages = submissions.GroupBy(
                        x => x.SubmitterId,
                        x => x.Feedback,
                        (key, g) => new
                        {
                            Id = key,
                            Feedbacks = g.ToList()
                        }).Select(x =>
                        new
                        {
                            x.Id,
                            Averages = new List<double>()
                            {
                                CalculateAverageLineCoveragePercentage(x.Feedbacks),
                                CalculateAverageBranchCoveragePercentage(x.Feedbacks),
                                CalculateAverageConditionalCoveragePercentage(x.Feedbacks),
                                CalculateAverageRedundantCoverage(x.Feedbacks),
                            },
                            Amount = x.Feedbacks.Count,
                        })
                    .ToList();
                studentAverages.ForEach(sa => 
                    fileText += $"{asg.Name}," +
                                $"{asg.Course.CourseName}," +
                                $"S{sa.Id}," +
                                $"{sa.Averages[0]}," +
                                $"{sa.Averages[1]}," +
                                $"{sa.Averages[2]}," +
                                $"{sa.Averages[3]}," +
                                $"{sa.Amount}\n" 
                );
            });

            var bytes = Encoding.ASCII.GetBytes(fileText);
            return new FileStreamResult(new MemoryStream(bytes), "text/csv")
            {
                FileDownloadName = "analysis.csv",
            };
        }

        public GroupBarChart GetChart()
        {
            var chart = new GroupBarChart()
            {
                Id = "overall",
                Minimum = 0,
                Maximum = 100,
                Colors = new List<string>()
                {
                    RandomColor(),
                    RandomColor(),
                    RandomColor(),
                    RandomColor(),
                },
                Groups = Assignments.Select(x => $"{x.Name} {x.Course.CourseName}").ToList(),
                SubGroups = new List<string>()
                {
                    "Line",
                    "Branch",
                    "Conditional",
                    "Redundancies",
                },
                Values = Assignments.Select(GetAssignmentAverages).ToList()
            };

            return chart;
        }

        private IList<int> GetAssignmentAverages(Assignment arg)
        {
            var submissions = GetSubmissions(Instructors, arg);

            var studentAverages = submissions.GroupBy(
                x => x.SubmitterId,
                x => x.Feedback,
                (key, g) => new
                {
                    Id = key,
                    Feedbacks = g.ToList()
                }).Select(x => new List<double>()
                {
                    CalculateAverageLineCoveragePercentage(x.Feedbacks),
                    CalculateAverageBranchCoveragePercentage(x.Feedbacks),
                    CalculateAverageConditionalCoveragePercentage(x.Feedbacks),
                    CalculateAverageRedundantCoverage(x.Feedbacks),
                })
                .ToList();

            if (studentAverages.Count == 0)
            {
                return new List<int>()
                {
                    0,
                    0,
                    0,
                    0,
                };
            }
            return new List<int>()
            {
                (int)Math.Floor(studentAverages.Select(x => x[0]).Average()),
                (int)Math.Floor(studentAverages.Select(x => x[1]).Average()),
                (int)Math.Floor(studentAverages.Select(x => x[2]).Average()),
                (int)Math.Floor(studentAverages.Select(x => x[3]).Average()),
            };
        }

        private double CalculateAverageRedundantCoverage(List<Feedback> feedback)
        {
            if (feedback.Count == 0) return 0;
            return feedback.Select(CalculateRedundantCoverage).Average();
        }

        private double CalculateRedundantCoverage(Feedback feedback)
        {
            var set = new HashSet<string>();
            feedback.InstructorTestResults.ToList().ForEach(
                i =>
                {
                    i.StudentTestResults.Skip(1).ToList().ForEach(
                        s => set.Add(s.TestName)
                    );
                }
            );
            return set.Count;
        }

        private double CalculateAverageBranchCoveragePercentage(List<Feedback> feedback)
        {
            if (feedback.Count == 0) return 0;
            return feedback.Select(CalculateBranchCoveragePercentage).Average();
        }

        private double CalculateAverageConditionalCoveragePercentage(List<Feedback> feedback)
        {
            if (feedback.Count == 0) return 0;
            return feedback.Select(CalculateConditionalCoveragePercentage).Average();
        }

        private double CalculateAverageLineCoveragePercentage(List<Feedback> feedback)
        {
            if (feedback.Count == 0) return 0;
            return feedback.Select(CalculateLineCoveragePercentage).Average();
        }

        private double CalculateConditionalCoveragePercentage(Feedback feedback)
        {
            var covered = feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.ConditionsCovered));
            var missed = feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.ConditionsMissed));
            if (covered + missed == 0) return 100;
            var percentage = (double)covered / (covered + missed);
            return percentage * 100.0f;
        }

        private double CalculateBranchCoveragePercentage(Feedback feedback)
        {
            var covered = feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.BranchesCovered));
            var missed = feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.BranchesMissed));
            if (covered + missed == 0) return 100;
            var percentage = (double)covered / (covered + missed);
            return percentage * 100.0f;
        }

        private double CalculateLineCoveragePercentage(Feedback feedback)
        {
            var covered = feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.LinesCovered));
            var missed = feedback.ClassCoverages.Sum(c => c.MethodCoverages.Sum(m => m.LinesMissed));
            if (covered + missed == 0) return 100;
            var percentage = (double)covered / (covered + missed);
            return percentage * 100.0f;
        }

        public IList<Submission> GetSubmissions(IList<string> instructors, Assignment assignment)
        {
            return Context.Submissions
                .Where(s => instructors.All(i => !i.Equals(s.SubmitterId)))
                .Where(s => s.AssignmentId.Equals(assignment.Id))
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


        public string RandomColor()
        {
            return "#" + Random.Next(255).ToString("X2") + Random.Next(255).ToString("X2") + Random.Next(255).ToString("X2");
        }

    }
}