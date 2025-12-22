using AutoMapper;
using Fota.DataLayer.Models;
using SharedProjectDTOs.DiagnosticSolutionDTOs;

namespace Fota.Mapping_profiles
{
    public class DiagnosticSolutionProfile : Profile
    {
        public DiagnosticSolutionProfile()
        {
            CreateMap<DiagnosticSolution, DiagnosticSolutionGetDto>()
                .ForMember(d => d.DiagnosticTitle, opt => opt.MapFrom(s => s.Diagnostic.Title))
                .ForMember(d => d.DeveloperName, opt => opt.MapFrom(s => s.Developer.Name));

            CreateMap<DiagnosticSolution, DiagnosticSolutionListDto>()
                .ForMember(d => d.DiagnosticTitle, opt => opt.MapFrom(s => s.Diagnostic.Title))
                .ForMember(d => d.DeveloperName, opt => opt.MapFrom(s => s.Developer.Name));

            CreateMap<DiagnosticSolutionCreateDto, DiagnosticSolution>().ReverseMap();
            CreateMap<DiagnosticSolutionUpdateDto, DiagnosticSolution>();
        }
    }
}
