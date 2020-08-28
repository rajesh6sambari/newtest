using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestingTutor.Dev.Data.DataAccess;
using TestingTutor.Dev.Data.Models;
using TestingTutor.UI.Data.ViewModels;

namespace TestingTutor.UI.Pages.SuperAdmin
{
    [Authorize(Policy = "SuperAdminPolicy")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IList<ClaimsViewModel> UserClaims { get; set; } = new List<ClaimsViewModel>()
        {
            new ClaimsViewModel { ClaimName = "Student", ClaimFriendlyName = "Student", IsChecked = false },
            new ClaimsViewModel { ClaimName = "Instructor", ClaimFriendlyName = "Instructor", IsChecked = false },
            new ClaimsViewModel { ClaimName = "InstitutionalAdmin", ClaimFriendlyName = "Institutional Administrator", IsChecked = false },
            new ClaimsViewModel { ClaimName = "SuperAdmin", ClaimFriendlyName = "Super Admin", IsChecked = false }
        };

        [BindProperty]
        public ApplicationUserViewModel ApplicationUserViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUserViewModel = new ApplicationUserViewModel{ UserName = id };

            if (ApplicationUserViewModel == null)
            {
                return NotFound();
            }

            await InitUserClaimsAsync();
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(ApplicationUser).State = EntityState.Modified;

            try
            {
                await PersistUserClaims();

                //await _context.SaveChangesAsync();
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

            return RedirectToPage("./Index", new { message = $"User {ApplicationUserViewModel.UserName} must log off the system and log back in for the changes to take effect."});
        }

        private async Task InitUserClaimsAsync()
        {
            var user = _userManager.Users.Single(x => x.Email.Equals(ApplicationUserViewModel.UserName));
            var claims = await _userManager.GetClaimsAsync(user);

            foreach (var claim in claims)
            {
                if (!claim.Type.Equals(ClaimTypes.Role)) continue;
                var findClaim = UserClaims.First(c => c.ClaimName.Equals(claim.Value));
                findClaim.IsChecked = true;
            }
        }

        private async Task PersistUserClaims()
        {
            var user = _userManager.Users.Single(x => x.Email.Equals(ApplicationUserViewModel.UserName));

            foreach (var claimViewModel in UserClaims)
            {
                var findClaim = (await _userManager.GetClaimsAsync(user)).ToList().FirstOrDefault(c => c.Value.Equals(claimViewModel.ClaimName));
                if (claimViewModel.IsChecked)
                {
                    if (findClaim == null)
                    {
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, claimViewModel.ClaimName));
                    }
                }
                else
                {
                    if (findClaim == null) continue;
                    await _userManager.RemoveClaimAsync(user, findClaim);
                }
            }
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
