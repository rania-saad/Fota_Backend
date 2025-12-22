using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

    namespace SharedProjectDTOs.Subscribers
    {
        public class SubscriberCreateDto
        {
            [Required]
            [MaxLength(100)]
            public string Name { get; set; } = null!;

            [Required]
            [MaxLength(256)]
            [EmailAddress]
            public string Email { get; set; } = null!;

            [MaxLength(20)]
            public string? PhoneNumber { get; set; }
        }

        public class SubscriberGetDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string? PhoneNumber { get; set; }
            public bool IsActive { get; set; }
            public List<int> TopicSubscriptionIds { get; set; } = new List<int>();
            public List<int> DiagnosticIds { get; set; } = new List<int>();
            public List<int> MessageDeliveryIds { get; set; } = new List<int>();
        }

        public class SubscriberUpdateDto
        {
            [Required]
            [MaxLength(100)]
            public string Name { get; set; } = null!;

            [MaxLength(20)]
            public string? PhoneNumber { get; set; }

            [Required]
            public bool IsActive { get; set; }
        }

        public class SubscriberTopicDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public bool IsActive { get; set; }
        }
    }

