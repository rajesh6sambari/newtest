using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.TestConcepts
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TestConcept> TestConcept { get;set; }

        public async Task OnGetAsync()
        {
            TestConcept = await _context.TestConcepts.ToListAsync();
        }
    }
}
