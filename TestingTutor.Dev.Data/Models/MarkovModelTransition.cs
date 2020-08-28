using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class MarkovModelTransition : IdentityModel<int>
    {
        [Required, DisplayName("Transition To State")]
        public int To { get; set; }
        [Required, DisplayName("Probablity of Transition")]
        public double Probability { get; set; }
        public string AnchorToTag => $"state_{To}";
    }
}