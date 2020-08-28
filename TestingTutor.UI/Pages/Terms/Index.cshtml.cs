using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.Terms
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Term> Term { get;set; }

        public async Task OnGetAsync()
        {
            var institutionalId = _context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
            Term = await _context.Terms.Where(c => c.InstitutionId.Equals(institutionalId)).ToListAsync();
        }
    }
}
