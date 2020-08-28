using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;

namespace TestingTutor.UI.Pages.TestConcepts
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TestConcept TestConcept { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TestConcepts.Add(TestConcept);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}