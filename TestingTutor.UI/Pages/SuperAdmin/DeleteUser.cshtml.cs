using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;

namespace TestingTutor.UI.Pages.SuperAdmin
{
    [Authorize(Policy = "SuperAdminPolicy")]
    public class DeleteUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       
        [BindProperty]
        public ApplicationUserViewModel ApplicationUserViewModel { get; set; }
        public object Users { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUserViewModel = new ApplicationUserViewModel { UserName = id };

            if (ApplicationUserViewModel == null)
            {
                return NotFound();
            }   

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = _userManager.Users.Single(x => x.Email.Equals(id));
                await _userManager.DeleteAsync(user);
            }


            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(ApplicationUserViewModel.UserName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { message = $"User {id} has been successfully deleted ." });
        }      

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
