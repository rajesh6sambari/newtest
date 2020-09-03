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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DevelopmentConcept).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DevConceptExists(DevelopmentConcept.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DevConceptExists(int id)
        {
            return _context.TestConcepts.Any(e => e.Id == id);
        }
    }
}
