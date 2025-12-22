using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProjectDTOs.Admin;



public class AdminGetDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public List<int> AssignedDiagnosticsIDs { get; set; }
    public List<string> AssignedDiagnosticsTitles { get; set; }

    public List<int> CreatedTeamsIDs { get; set; }
    public List<string> CreatedTeamsNames { get; set; }
}

///////
///
// Admin DTOs
public class AdminCreateDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}

public class AdminUpdateDto
{
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}








