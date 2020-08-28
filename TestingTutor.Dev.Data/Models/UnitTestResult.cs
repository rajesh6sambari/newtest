using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class UnitTestResult : IdentityModel<int>
    {
        [Required]
        public int UnitTestId { get; set; }
        [DisplayName("Unit Test")]
        public virtual UnitTest UnitTest { get; set; }
        [Required, DisplayName("Passed?")]
        public bool Passed { get; set; }
    }
}
