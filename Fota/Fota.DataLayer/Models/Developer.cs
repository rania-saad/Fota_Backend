using Fota.DataLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fota.Models
{
    public class Developer : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string? IdentityUserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<BaseMessage> UploadedMessages { get; set; } = new List<BaseMessage>();
        public virtual ICollection<BaseMessage> PublishedMessages { get; set; } = new List<BaseMessage>();
        public virtual ICollection<TeamDeveloper> TeamMemberships { get; set; } = new List<TeamDeveloper>();
        public virtual ICollection<Team> LeadingTeams { get; set; } = new List<Team>();
        public virtual ICollection<Diagnostic> AssignedDiagnostics { get; set; } = new List<Diagnostic>();
    }

}
