using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.TestConcepts
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TestConcept TestConcept { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TestConcept = await _context.TestConcepts.FirstOrDefaultAsync(m => m.Id == id);

            if (TestConcept == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TestConcept = await _context.TestConcepts.FindAsync(id);

            if (TestConcept != null)
            {
                _context.TestConcepts.Remove(TestConcept);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
