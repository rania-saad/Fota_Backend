using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProjectDTOs.Developers;

// Developer DTOs
public class DeveloperCreateDto
{

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}

public class DeveloperGetDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    public List<int> UploadedMessagesIDs { get; set; }
    public List<int> PublishedMessagesIDs { get; set; }
    public List<int> LeadingTeamsIDs { get; set; }
    public List<int> AssignedDiagnosticsIDs { get; set; }
}

public class DeveloperUpdateDto
{
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }
}

public class DeveloperTeamDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}
