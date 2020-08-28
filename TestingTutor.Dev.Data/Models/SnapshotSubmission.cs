using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class SnapshotSubmission : IdentityModel<int>
    {
        [Required, DisplayName("Created Date")]
        public DateTime CreatedDateTime { get; set; }
        [Required, DisplayName("Snapshot's Files")]
        public byte[] Files { get; set; }
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
