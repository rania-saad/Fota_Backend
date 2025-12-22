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
    public class MessageDelivery : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        // Foreign Keys
        [Required]
        public int MessageId { get; set; }

        [Required]
        public int SubscriberId { get; set; }

        // Delivery tracking
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsDelivered { get; set; } = false;
        public bool IsRead { get; set; } = false;
        public bool IsFailed { get; set; } = false;

        [MaxLength(500)]
        public string? FailureReason { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(MessageId))]
        public virtual BaseMessage Message { get; set; } = null!;

        [ForeignKey(nameof(SubscriberId))]
        public virtual Subscriber Subscriber { get; set; } = null!;
    }
}
