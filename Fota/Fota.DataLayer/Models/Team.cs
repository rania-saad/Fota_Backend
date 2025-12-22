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
    public class Team : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Foreign Keys
        public int? LeadId { get; set; }
        public int? CreatedByAdminId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(LeadId))]
        public virtual Developer? Lead { get; set; }

        [ForeignKey(nameof(CreatedByAdminId))]
        public virtual Admin? CreatedByAdmin { get; set; }

        public virtual ICollection<TeamDeveloper> TeamDevelopers { get; set; } = new List<TeamDeveloper>();
        public virtual ICollection<TeamTopic> TeamTopics { get; set; } = new List<TeamTopic>();
    }

}
