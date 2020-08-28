using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestingTutor.Dev.Data.Models;

namespace TestingTutor.UI.Pages.SuperAdmin
{
    [Authorize(Policy = "SuperAdminPolicy")]
    public class UserManagementModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IList<ApplicationUser> Users { get; set; }
        public string EditMessage { get; set; }

        public UserManagementModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public void OnGet(string message = null)
        {
            EditMessage = message;
            Users = _userManager.Users.ToList();
        }
    }
}