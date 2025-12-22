using Fota.DataLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fota.Models
{

    public class Topic : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<TopicSubscriber> TopicSubscribers { get; set; } = new List<TopicSubscriber>();
        public virtual ICollection<BaseMessage> Messages { get; set; } = new List<BaseMessage>();
        public virtual ICollection<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();
        public virtual ICollection<TeamTopic> TeamTopics { get; set; } = new List<TeamTopic>();
    }
}
