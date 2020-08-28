using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestingTutor.UI.Pages
{
    [Authorize(Policy = "InstructorsOnly")]
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "About page...";
        }
    }
}
