using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var institutionalId = _context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
            ViewData["TermId"] = new SelectList(_context.Terms.Where(t => t.InstitutionId.Equals(institutionalId)), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var institutionId = _context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
            Course.InstitutionId = institutionId;

            _context.Courses.Add(Course);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}