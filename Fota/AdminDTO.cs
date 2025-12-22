using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GlopalDTOs
{
    public class AdminDTO
    {

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
            public string Email { get; set; } = null!;

        }

        public class AdminGetDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }

            public List<int> AssignedDiagnosticsIDs { get; set; }
            public List<string>  AssignedDiagnosticsTitles { get; set; }

            public List<int> CreatedTeamsIDs { get; set; }
            public List<string> CreatedTeamsNames { get; set; }
        }


        
    }
}   



