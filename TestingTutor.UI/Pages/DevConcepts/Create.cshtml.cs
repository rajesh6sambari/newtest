using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.DevConcepts
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
        public TestConcept DevelopmentConcept { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TestConcepts.Add(DevelopmentConcept);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
