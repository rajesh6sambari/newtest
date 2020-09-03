using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.DevConcepts
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
        public TestConcept DevelopmentConcept { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DevelopmentConcept = await _context.TestConcepts.FirstOrDefaultAsync(m => m.Id == id);

            if (DevelopmentConcept == null)
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

            DevelopmentConcept = await _context.TestConcepts.FindAsync(id);

            if (DevelopmentConcept != null)
            {
                _context.TestConcepts.Remove(DevelopmentConcept);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
