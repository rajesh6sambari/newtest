using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.Submissions
{
    [Authorize]
    public class IndexModel : SubmissionsBase
    {
        public IndexModel(ApplicationDbContext context)
            :base (context)
        {
        }

        public IList<Submission> Submission { get;set; }

        public async Task OnGetAsync()
        {
            var user = _context.Users.AsNoTracking().Single(u => u.UserName.Equals(User.Identity.Name));

            var submission = await _context.Submissions.AsNoTracking().Where(s => UserOwnsThisSubmission(s, user) == true)
                .Include(s => s.Assignment)
                .ThenInclude(s => s.Course)
                .Include(s => s.Feedback)
                .ToListAsync();

            Submission = GetLastSubmissions(submission);
        }

        public IList<Submission> GetLastSubmissions(IList<Submission> submissions)
        {
            return submissions.GroupBy(s => s.AssignmentId)
                .Select(g => g.OrderBy(a => a.SubmissionDateTime).Last()).ToList();
        }


        public int GetIndex(Submission submission)
        {
            return _context.Submissions.AsNoTracking()
                            .Count(s => s.SubmitterId.Equals(submission.SubmitterId) && s.AssignmentId == submission.AssignmentId) - 1;
        }

    }
}
