using AutoMapper;
using Fota.DataLayer.Models;
using SharedProjectDTOs.Admin;

namespace Fota.Mapping_profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()  // <-- يجب أن يكون هذا الكونستركتور
        {
            CreateMap<Admin, AdminGetDTO>()
                .ForMember(
                    dest => dest.AssignedDiagnosticsIDs,
                    opt => opt.MapFrom(src => src.AssignedDiagnostics.Select(d => d.Id).ToList())
                )
                .ForMember(
                    dest => dest.AssignedDiagnosticsTitles,
                    opt => opt.MapFrom(src => src.AssignedDiagnostics.Select(d => d.Title).ToList())
                )
                .ForMember(
                    dest => dest.CreatedTeamsIDs,
                    opt => opt.MapFrom(src => src.CreatedTeams.Select(t => t.Id).ToList())
                )
                .ForMember(
                    dest => dest.CreatedTeamsNames,
                    opt => opt.MapFrom(src => src.CreatedTeams.Select(t => t.Name).ToList())
                );
        }
    }
}
