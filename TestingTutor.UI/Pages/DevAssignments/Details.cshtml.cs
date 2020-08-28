using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DetailsModel : PageModel
    {
        public TestingTutorProjectContext Context;
        public DbSet<DevAssignment> Assignments;

        public DetailsModel(TestingTutorProjectContext context)
        {
            Context = context;
            Assignments = context.Set<DevAssignment>();
        }

        [FromRoute]
        public int Id { get; set; }

        public DevAssignment Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            Context.Entry(Assignment).Reference(x => x.CourseClass).Load();
            Context.Entry(Assignment).Reference(x => x.Solution)
                .Query().Include(x => x.MethodDeclarations).Load();
            Context.Entry(Assignment).Reference(x => x.TestProject)
                .Query().Include(x => x.UnitTests).Load();

            return Page();
        }
    }
}