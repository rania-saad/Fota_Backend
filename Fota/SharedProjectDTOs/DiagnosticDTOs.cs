using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProjectDTOs.DiagnosticDTOs
{
    using Fota.DataLayer.Enum;
    using Fota.DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace SharedProjectDTOs.Diagnostics
    {
        // Create DTO
        public class DiagnosticCreateDto
        {
            [Required]
            [MaxLength(200)]
            public string Title { get; set; } = null!;

            [MaxLength(2000)]
            public string? Description { get; set; }

            [Required]
            //[MaxLength(50)]
            public DiagnosticPriority Priority { get; set; } = DiagnosticPriority.Medium;

            [Required]
            public int SubscriberId { get; set; }

            public string TopicName { get; set; }

            public string CarModel { get; set; }

            public string CarBrand { get; set; }
        }

        // Get DTO - Full details
        public class DiagnosticGetDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public string? Description { get; set; }
            public DiagnosticPriority Priority { get; set; }
            public DiagnosticStatus Status { get; set; } 
            public DateTime? ResolvedAt { get; set; }
            public DateTime? ClosedAt { get; set; }
            public int SubscriberId { get; set; }
            public string? SubscriberName { get; set; }
            public int? TopicId { get; set; }
            public string? TopicName { get; set; }
            public int? AssignedByAdminId { get; set; }
            public string? AssignedByAdminName { get; set; }
            public int? AssignedToDeveloperId { get; set; }
            public string? AssignedToDeveloperName { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public string CarModel { get; set; }

            public string CarBrand { get; set; }
            public List<int> SolutionIds { get; set; } = new List<int>();
        }

        // List DTO - Lightweight for listing
        public class DiagnosticListDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public DiagnosticPriority Priority { get; set; } 
            public DiagnosticStatus Status { get; set; }
            public string? SubscriberName { get; set; }
            public string? TopicName { get; set; }
            public string CarModel { get; set; }

            public string CarBrand { get; set; }
            public string? AssignedToDeveloperName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        // Update DTO
        public class DiagnosticUpdateDto
        {
            [MaxLength(200)]
            public string? Title { get; set; }

            [MaxLength(2000)]
            public string? Description { get; set; }

            [MaxLength(50)]
            public DiagnosticPriority Priority { get; set; }
            public string CarModel { get; set; }

            public string CarBrand { get; set; }

            public int? TopicId { get; set; }
        }

        // Assignment DTO
        public class DiagnosticAssignDto
        {
            [Required]
            public int DeveloperId { get; set; }

            [Required]
            public int AdminId { get; set; }
        }

        // Status Update DTO
        public class DiagnosticStatusUpdateDto
        {
            [Required]
            [MaxLength(50)]
            public DiagnosticStatus Status { get; set; } 
        }
    }
}
