using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.UI.Data;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.Submissions
{
    public class DetailsModel : PageModel
    {
        private ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Submission Submission { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Submission = await _context.Submissions
                .Include(s => s.Assignment)
                .Include(s => s.Feedback)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.InstructorTestResults)
                .ThenInclude(s => s.StudentTestResults)
                .ThenInclude(s=> s.TestCaseStatus)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.InstructorTestResults)
                .ThenInclude(s => s.TestResultConcepts)
                .Include(s => s.Feedback)
                .ThenInclude(s => s.InstructorTestResults)
                .ThenInclude(s => s.TestCaseStatus)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Submission == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
