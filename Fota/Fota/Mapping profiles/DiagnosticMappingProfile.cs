using AutoMapper;
using Fota.DataLayer.Models;
using Fota.Models;
using SharedProjectDTOs.DiagnosticDTOs.SharedProjectDTOs.Diagnostics;

namespace Fota.Mapping_profiles
{
    public class DiagnosticMappingProfile : Profile
    {
        public DiagnosticMappingProfile()
        {
            // Diagnostic -> DiagnosticGetDto
            CreateMap<Diagnostic, DiagnosticGetDto>()
                .ForMember(dest => dest.SubscriberName, opt => opt.MapFrom(src => src.Subscriber != null ? src.Subscriber.Name : null))
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic != null ? src.Topic.Name : null))
                .ForMember(dest => dest.AssignedByAdminName, opt => opt.MapFrom(src => src.AssignedByAdmin != null ? src.AssignedByAdmin.Name : null))
                .ForMember(dest => dest.AssignedToDeveloperName, opt => opt.MapFrom(src => src.AssignedToDeveloper != null ? src.AssignedToDeveloper.Name : null))
                .ForMember(dest => dest.SolutionIds, opt => opt.MapFrom(src => src.Solutions.Select(s => s.Id).ToList()));

            // Diagnostic -> DiagnosticListDto
            CreateMap<Diagnostic, DiagnosticListDto>()
                .ForMember(dest => dest.SubscriberName, opt => opt.MapFrom(src => src.Subscriber != null ? src.Subscriber.Name : null))
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic != null ? src.Topic.Name : null))
                .ForMember(dest => dest.AssignedToDeveloperName, opt => opt.MapFrom(src => src.AssignedToDeveloper != null ? src.AssignedToDeveloper.Name : null));

            // DiagnosticCreateDto -> Diagnostic
            CreateMap<DiagnosticCreateDto, Diagnostic>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Open"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}