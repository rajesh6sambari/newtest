using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Submissions
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class StudentFeedbackModel : SubmissionsBase
    {
        public StudentFeedbackModel(ApplicationDbContext context) : base(context)
        {
        }

        public IList<Submission> Submissions { get; set; }

        public Assignment Assignment { get; set; }

        public int Index { get; set; }

        public Submission Submission { get; set; }

        public int MediumCoverage { get; set; }
        public int HighCoverage { get; set; }


        public async Task<IActionResult> OnGetAsync(int index, int id, string submitter)
        {
            Index = index;

            Submissions = await _context.Submissions.AsNoTracking().Where(s => s.SubmitterId.Equals(submitter))
                .Include(s => s.Assignment)
                .ThenInclude(s => s.FeedbackLevelOption)
                .Include(s => s.Assignment)
                .ThenInclude(s => s.AssignmentApplicationModes)
                .ThenInclude(s => s.ApplicationMode)
                .Include(s => s.Assignment)
                .ThenInclude(s => s.AssignmentCoverageTypeOptions)
                .ThenInclude(s => s.CoverageTypeOption)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.InstructorTestResults)
                .ThenInclude(s => s.StudentTestResults)
                .ThenInclude(s => s.TestCaseStatus)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.InstructorTestResults)
                .ThenInclude(s => s.TestResultConcepts)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.InstructorTestResults)
                .ThenInclude(s => s.TestCaseStatus)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.EngineException)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.ClassCoverages)
                .ThenInclude(s => s.MethodCoverages)
                .Where(s => s.AssignmentId.Equals(id))
                .ToListAsync();

            if (!Submissions.Any()) return NotFound();

            if (index < 0 || index >= Submissions.Count) return NotFound();

            Assignment = Submissions[0].Assignment;

            Submission = Submissions[index];

            HighCoverage = (int)Assignment.TestCoverageLevel;
            MediumCoverage = (int)(Assignment.TestCoverageLevel * 0.75);

            return Page();
        }

        public int CalculateTestCoveredPercentage(Submission submission)
        {
            var total = submission.Feedback.InstructorTestResults.Count;
            if (total == 0) return 100;

            var covered = submission.Feedback.InstructorTestResults.Count(s => s.StudentTestResults.Count > 0);

            return (int)Math.Floor(((double)covered / total) * 100);
        }

        public int CalculateNumberOfRedundantTest(Submission submission)
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

        public async Task<FileStreamResult> OnPostAsync(int id, string type)
        {
            var submission = await _context.Submissions.FindAsync(id);

            switch (type)
            {
                case "Solution":
                    return new FileStreamResult(new MemoryStream(submission.SubmitterSolution), "application/zip");
                case "TestSolution":
                    return new FileStreamResult(new MemoryStream(submission.SubmitterTestCaseSolution), "application/zip");
            }
            return null;
        }

    }
}