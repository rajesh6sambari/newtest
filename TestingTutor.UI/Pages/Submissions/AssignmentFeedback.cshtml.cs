using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Submissions
{
    [Authorize]
    public class AssignmentFeedbackModel : SubmissionsBase
    {
        public AssignmentFeedbackViewModel ViewModel { get; }

        public AssignmentFeedbackModel(ApplicationDbContext context, AssignmentFeedbackViewModel viewModel) : base(context)
        {
            ViewModel = viewModel;
        }

        public IActionResult OnGet(int index, int id)
        {
            ViewModel.Index = index;
            ViewModel.Submissions = GetSubmissions(id);
            if (index < 0 || index >= ViewModel.Submissions.Count)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int index, int id, string notes)
        {
            ViewModel.Index = index;
            ViewModel.Submissions = GetSubmissions(id);
            if (index < 0 || index >= ViewModel.Submissions.Count)
                return NotFound();

            ViewModel.Submission.Notes = notes;
            _context.Submissions.Update(ViewModel.Submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("./AssignmentFeedback3", new
            {
                index,
                id
            });
        }

        public IList<Submission> GetSubmissions(int id)
        {
            var user = GetUser();

            var submissions = _context.Submissions.Where(s => UserOwnsThisSubmission(s, user))
                .Where(s => s.AssignmentId.Equals(id)).ToList();

            submissions.ForEach(s =>
            {
                _context.Entry(s).Reference(x => x.Assignment).Query()
                    .Include(x => x.FeedbackLevelOption)
                    .Include(x => x.AssignmentApplicationModes)
                    .ThenInclude(x => x.ApplicationMode)
                    .Include(x => x.AssignmentCoverageTypeOptions)
                    .ThenInclude(x => x.CoverageTypeOption).Load();

                _context.Entry(s).Reference(x => x.Feedback).Query()
                    .Include(x => x.InstructorTestResults)
                    .ThenInclude(x => x.StudentTestResults)
                    .ThenInclude(x => x.TestCaseStatus)
                    .Include(x => x.InstructorTestResults)
                    .ThenInclude(x => x.TestResultConcepts)
                    .ThenInclude(x => x.TestConcept)
                    .Include(x => x.InstructorTestResults)
                    .ThenInclude(x => x.TestCaseStatus)
                    .Include(x => x.EngineException)
                    .Include(x => x.ClassCoverages)
                    .ThenInclude(x => x.MethodCoverages)
                    .Load();
            });

            return submissions;
        }

        public ApplicationUser GetUser()
        {
            return _context.Users.AsNoTracking().Single(u => u.UserName.Equals(User.Identity.Name));
        }
    }
}