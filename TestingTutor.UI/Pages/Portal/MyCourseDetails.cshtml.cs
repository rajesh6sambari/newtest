using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.Portal
{
    [Authorize]
    public class MyCourseDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MyCourseDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }
        public List<Assignment> Assignments { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses
                .Include(c => c.Institution)
                .Include(c => c.Term).FirstOrDefaultAsync(m => m.Id == id);

            Assignments = await _context.Assignments.AsNoTracking()
                .Include(assignment => assignment.Course)
                .Include(assignment => assignment.Language)
                .Where(assignment => assignment.CourseId.Equals(id))
                .ToListAsync();

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }

        [HttpPost, ActionName("Download")]
        public async Task<FileStreamResult> OnPostDownload(int id, string documentType)
        {
            var assignment = await _context.GetAssignmentById(id);
            var fileBytes = await _context.GetDocumentFileBytes(assignment.Id, documentType);

            var mimeType = string.Empty;

            switch (documentType)
            {
                case "AssignmentSpecification":
                    mimeType = "application/pdf";
                    break;
                case "ReferenceSolution":
                    mimeType = "application/zip";
                    break;
                case "ReferenceTestCasesSolutions":
                    mimeType = "application/zip";
                    break;
            }

            return new FileStreamResult(new MemoryStream(fileBytes), mimeType);
        }
    }
}
