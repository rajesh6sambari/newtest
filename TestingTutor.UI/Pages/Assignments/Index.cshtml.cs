using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Data.DataAccess;
namespace TestingTutor.UI.Pages.Assignments
{
    [Authorize(Policy = "InstructorAndHigherPolicy")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Assignment> Assignment { get;set; }

        public async Task OnGetAsync()
        {
            var institutionalId = _context.Users.Single(u => u.Email.Equals(User.Identity.Name)).InstitutionId;
            Assignment = (await _context.GetAssignmentsAsync()).Where(a => a.InstitutionId.Equals(institutionalId)).ToList();
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

            return  new FileStreamResult(new MemoryStream(fileBytes), mimeType);
        }
    }
}
