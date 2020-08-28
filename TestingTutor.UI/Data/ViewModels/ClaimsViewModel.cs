using System.Security.Claims;

namespace TestingTutor.UI.Data.ViewModels
{
    public class ClaimsViewModel
    {
        public string ClaimName { get; set; }
        public string ClaimFriendlyName { get; set; }
        public bool IsChecked { get; set; }
    }
}
