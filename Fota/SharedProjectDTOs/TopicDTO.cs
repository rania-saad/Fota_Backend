using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedProjectDTOs.Topics;

// Topic DTOs
public class TopicCreateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class TopicUpdateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class TopicGetDTO
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

    public List<int> TopicSubscribersIDs { get; set; }
    public List<int> MessagesIDs { get; set; }
    public List<int> DiagnosticsIDs { get; set; }
    public List<int> TeamTopicsIDs { get; set; }


}
