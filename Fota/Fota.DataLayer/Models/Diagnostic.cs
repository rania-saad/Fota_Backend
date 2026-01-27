using Fota.DataLayer.Enum;
using Fota.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Models
{
    public class Diagnostic : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public DiagnosticPriority Priority { get; set; } // Low, Medium, High, Critical

        [MaxLength(50)]
        public DiagnosticStatus Status { get; set; } // Open, InProgress, Resolved, Closed

        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // Foreign Keys
        [Required]
        public int SubscriberId { get; set; }

        public string CarModel {  get; set; }

        public string CarBrand {  get; set; }
        public int? TopicId { get; set; }
        public int? AssignedByAdminId { get; set; }
        public int? AssignedToDeveloperId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(SubscriberId))]
        public virtual Subscriber Subscriber { get; set; } = null!;

        [ForeignKey(nameof(TopicId))]
        public virtual Topic? Topic { get; set; }

        [ForeignKey(nameof(AssignedByAdminId))]
        public virtual Admin? AssignedByAdmin { get; set; }

        [ForeignKey(nameof(AssignedToDeveloperId))]
        public virtual Developer? AssignedToDeveloper { get; set; }

        public virtual ICollection<DiagnosticSolution> Solutions { get; set; } = new List<DiagnosticSolution>();
    }
}
