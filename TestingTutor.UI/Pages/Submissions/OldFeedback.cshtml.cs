using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;

namespace TestingTutor.UI.Pages.Submissions
{
    [Authorize]
    public class OldFeedbackModel : SubmissionsBase
    {
        public OldFeedbackModel(ApplicationDbContext context)
            : base(context)
        {

        }

        public Submission Submission { get; set; }
        public FeedbackViewModel FeedbackViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.AsNoTracking().Single(u => u.UserName.Equals(User.Identity.Name));

            Submission = await _context.Submissions.AsNoTracking()
                .Include(s => s.Assignment)
                .ThenInclude(s => s.FeedbackLevelOption)
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
                .FirstOrDefaultAsync(m => m.Id == id);

            FeedbackViewModel = new FeedbackViewModel(_context, Submission);

            if (Submission == null || !UserOwnsThisSubmission(Submission, user))
            {
                return NotFound();
            }
            return Page();
        }

        private async Task<bool> StudentOwnsThisSubmissionAsync()
        {
            var user = await _context.Users.FirstAsync(u => u.UserName.Equals(User.Identity.Name));
            return Submission.SubmitterId.Equals(user.Id);
        }
    }
}
