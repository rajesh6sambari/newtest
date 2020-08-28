using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.Submissions
{
    public class SubmissionsBase : PageModel
    {
        protected readonly ApplicationDbContext _context;

        public SubmissionsBase(ApplicationDbContext context)
        {
            _context = context;
        }

        protected bool UserOwnsThisSubmission(Submission submission, ApplicationUser user)
        {
            return submission.SubmitterId.Equals(user.Id);
        }
    }
}
