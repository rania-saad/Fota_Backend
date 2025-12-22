using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProjectDTOs.DiagnosticSolutionDTOs
{
    public class DiagnosticSolutionGetDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public byte[]? HexFileContent { get; set; }
        public string? HexFileName { get; set; }
        public string? Version { get; set; }
        public string Status { get; set; } = "Draft";

        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectionReason { get; set; }

        public int DiagnosticId { get; set; }
        public string? DiagnosticTitle { get; set; }

        public int DeveloperId { get; set; }
        public string? DeveloperName { get; set; }
    }

    public class DiagnosticSolutionListDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public string Status { get; set; } = "Submitted";
        public string? DiagnosticTitle { get; set; }
        public string? DeveloperName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DiagnosticSolutionCreateDto
    {
        public string? Description { get; set; }
        public byte[]? HexFileContent { get; set; }
        public string? HexFileName { get; set; }
        public string? Version { get; set; }
        public string Status { get; set; } = "Draft";

        public int DiagnosticId { get; set; }
        public int DeveloperId { get; set; }
    }

    public class DiagnosticSolutionUpdateDto
    {
        public string? Description { get; set; }
        public byte[]? HexFileContent { get; set; }
        public string? HexFileName { get; set; }
        public string? Version { get; set; }
        public int? DiagnosticId { get; set; }
    }

    public class DiagnosticSolutionApproveDto
    {
        public int ApprovedByDeveloperId { get; set; }
    }

    public class DiagnosticSolutionRejectDto
    {
        public string RejectionReason { get; set; } = null!;
    }
}
