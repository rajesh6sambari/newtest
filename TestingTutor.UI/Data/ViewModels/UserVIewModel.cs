using System.ComponentModel.DataAnnotations;

namespace TestingTutor.UI.Data.ViewModels
{
    public class UserViewModel
    {
        [Display(Name="Add to Course")]
        public bool IsChecked { get; set; }
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
