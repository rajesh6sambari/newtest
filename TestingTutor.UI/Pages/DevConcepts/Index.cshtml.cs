using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.DevConcepts
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TestConcept> DevelopmentConcept { get; set; }

        public async Task OnGetAsync()
        {
            DevelopmentConcept = await _context.TestConcepts
                                        .Where(c => c.IsDevelopment.Equals(true))
                                        .ToListAsync();
        }
    }
}
