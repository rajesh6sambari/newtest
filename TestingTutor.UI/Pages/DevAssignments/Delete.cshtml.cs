using System.Threading.Tasks;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Constants;
using TestingTutor.UI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TestingTutor.UI.Pages.DevAssignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DeleteModel : PageModel
    {
        public DeleteHelper DeleteHelper;
        public TestingTutorProjectContext Context;
        public DbSet<DevAssignment> Assignments;
    }
}