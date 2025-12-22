using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedProjectDTOs.Teams
{
    public class TeamGetDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int? LeadId { get; set; }
        public string? LeadName { get; set; }   // لازم علشان AutoMapper

        public int? CreatedByAdminId { get; set; }
        public string? CreatedByAdminName { get; set; } // لازم علشان AutoMapper

        public List<int> TeamDeveloperIDs { get; set; } = new();
        public List<int> TeamTopicsIDs { get; set; } = new(); // لازم علشان AutoMapper
    }

    public class TeamCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? LeadId { get; set; }
        public int? CreatedByAdminId { get; set; }
    }

    public class TeamUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? LeadId { get; set; }
        public bool IsActive { get; set; }
    }

}

//public class TeamCreateDto
//{
//    public string Name { get; set; } = null!;
//    public string? Description { get; set; }
//    public int? LeadId { get; set; }
//    public int? CreatedByAdminId { get; set; }
//}

//public class TeamUpdateDto
//{
//    public string Name { get; set; } = null!;
//    public string? Description { get; set; }
//    public int? LeadId { get; set; }
//    public bool IsActive { get; set; }
//}
