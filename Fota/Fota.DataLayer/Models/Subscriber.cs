

using Fota.DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace Fota.Models
{
    public class Subscriber : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string location { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<TopicSubscriber> TopicSubscriptions { get; set; } = new List<TopicSubscriber>();
        public virtual ICollection<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();
        public virtual ICollection<MessageDelivery> MessageDeliveries { get; set; } = new List<MessageDelivery>();
    }
}
