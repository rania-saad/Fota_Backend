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
    public class DiagnosticSolution : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DiagnosticId { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

      //  public bool IsDeleted { get; set; } = false;

        public byte[]? HexFileContent { get; set; }

        [MaxLength(500)]
        public string? HexFileName { get; set; }

        [MaxLength(50)]
        public string? Version { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected

        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }

        [MaxLength(500)]
        public string? RejectionReason { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(DiagnosticId))]
        public virtual Diagnostic Diagnostic { get; set; } = null!;

        [ForeignKey(nameof(DeveloperId))]
        public virtual Developer Developer { get; set; } = null!;
    }
}
