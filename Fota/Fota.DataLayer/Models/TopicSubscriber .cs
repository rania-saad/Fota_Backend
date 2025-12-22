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
    public class TopicSubscriber : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TopicId { get; set; }

        [Required]
        public int SubscriberId { get; set; }

        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UnsubscribedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey(nameof(TopicId))]
        public virtual Topic Topic { get; set; } = null!;

        [ForeignKey(nameof(SubscriberId))]
        public virtual Subscriber Subscriber { get; set; } = null!;
    }
}
