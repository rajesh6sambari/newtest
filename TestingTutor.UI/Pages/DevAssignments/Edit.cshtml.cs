using System.Threading.Tasks;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TestingTutor.UI.Pages.DevAssignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class EditModel : PageModel
    {
        public TestingTutorProjectContext Context;
        public DbSet<DevAssignment> Assignments;

        public EditModel(TestingTutorProjectContext context)
        {
            Context = context;
            Assignments = context.Set<DevAssignment>();
        }

        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public DevAssignment Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);

            if (Assignment == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(Assignment.Name))
            {
                ModelState.AddModelError("Assignment.Name", "Assignment's name is required.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var assignment = await Assignments.FindAsync(Id);

            if (assignment == null) return NotFound();

            assignment.Name = Assignment.Name;

            Assignments.Update(assignment);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Assignments/Index", 
                new {id = assignment.CourseClassId});
        }
    }
}