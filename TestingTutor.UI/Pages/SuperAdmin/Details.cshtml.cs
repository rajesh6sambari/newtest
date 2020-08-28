using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.SuperAdmin
{
    [Authorize(Policy = "SuperAdminPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;        
        public IList<ApplicationUser> Users { get; set; }       
        
        [BindProperty]
        public DetailModelViewModel DetailModelViewModel { get; set; }

        public DetailsModel(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Users = _userManager.Users.Where(x => x.UserName == id).ToList();
                Users[0].Institution = _context.Institutions.Where(x => x.Id == Users[0].InstitutionId).First();
            }
            return Page();
        }
    }
}