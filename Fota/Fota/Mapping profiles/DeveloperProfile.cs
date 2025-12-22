using AutoMapper;
using Fota.DataLayer.Models;
using Fota.Models;
using SharedProjectDTOs;
using SharedProjectDTOs.Developers;

namespace Fota.Mapping_profiles
{
    public class DeveloperProfile : Profile
    {
        public DeveloperProfile()
        {
            CreateMap<Developer, DeveloperGetDTO>()
                    .ForMember(
                        dest => dest.UploadedMessagesIDs,
                        opt => opt.MapFrom(src => src.UploadedMessages.Select(d => d.Id).ToList())
                    )
                    .ForMember(
                        dest => dest.PublishedMessagesIDs,
                        opt => opt.MapFrom(src => src.PublishedMessages.Select(d => d.Id).ToList())
                    )
                    .ForMember(
                        dest => dest.LeadingTeamsIDs,
                        opt => opt.MapFrom(src => src.LeadingTeams.Select(t => t.Id).ToList())
                    )
                    .ForMember(
                        dest => dest.AssignedDiagnosticsIDs,
                        opt => opt.MapFrom(src => src.AssignedDiagnostics.Select(t => t.Id).ToList())
                    );


            CreateMap<Developer, DeveloperTeamDto>().ReverseMap();








        }
    }
}
