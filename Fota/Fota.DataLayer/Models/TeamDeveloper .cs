using Fota.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Models
{
    public class TeamDeveloper : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "Member"; // Member, Lead, Senior, Junior

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LeftAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey(nameof(TeamId))]
        public virtual Team Team { get; set; } = null!;

        [ForeignKey(nameof(DeveloperId))]
        public virtual Developer Developer { get; set; } = null!;
    }
}
