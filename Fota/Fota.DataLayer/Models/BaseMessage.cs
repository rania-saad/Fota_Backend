using Fota.DataLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fota.Models
{
    public class BaseMessage : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string MessageType { get; set; } = "Standard"; // Standard, Diagnostic, Broadcast

        [MaxLength(2000)]
        public string? Description { get; set; }

        //public byte[]? HexFileContent { get; set; }

        public string? HexFileContent { get; set; }


        [MaxLength(500)]
        public string? HexFileName { get; set; }

        [MaxLength(50)]
        public string? Version { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Draft, Pending, Approved, Published, Rejected

        public DateTime? ApprovedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? RejectedAt { get; set; }

        [MaxLength(500)]
        public string? RejectionReason { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Foreign Keys
        [Required]
        public int TopicId { get; set; }

        [Required]
        public int UploaderId { get; set; }

        public int? PublisherId { get; set; }
        public int? ApprovedById { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(TopicId))]
        public virtual Topic Topic { get; set; } = null!;

        [ForeignKey(nameof(UploaderId))]
        public virtual Developer Uploader { get; set; } = null!;

        [ForeignKey(nameof(PublisherId))]
        public virtual Developer? Publisher { get; set; }

        public virtual ICollection<MessageDelivery> Deliveries { get; set; } = new List<MessageDelivery>();
    }

}
