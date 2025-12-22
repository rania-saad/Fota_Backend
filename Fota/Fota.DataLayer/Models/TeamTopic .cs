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
    public class TeamTopic : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public int TopicId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey(nameof(TeamId))]
        public virtual Team Team { get; set; } = null!;

        [ForeignKey(nameof(TopicId))]
        public virtual Topic Topic { get; set; } = null!;
    }

}
