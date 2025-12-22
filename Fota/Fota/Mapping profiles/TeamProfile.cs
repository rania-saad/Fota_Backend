    using AutoMapper;
    using Fota.DataLayer.Models;
    using global::Fota.DataLayer.Models;
    using SharedProjectDTOs.Teams;

    namespace Fota.MappingProfiles
    {
        public class TeamProfile : Profile
        {
            public TeamProfile()
            {

            // Map TeamCreateDto -> Team
            CreateMap<TeamCreateDto, Team>();

            // Map TeamUpdateDto -> Team
            CreateMap<TeamUpdateDto, Team>();

            // Map Team -> TeamGetDTO
            CreateMap<Team, TeamGetDTO>()
                .ForMember(dest => dest.TeamDeveloperIDs,
                    opt => opt.MapFrom(src => src.TeamDevelopers.Select(td => td.DeveloperId).ToList()))
                .ForMember(dest => dest.TeamTopicsIDs,
                    opt => opt.MapFrom(src => src.TeamTopics.Select(tt => tt.TopicId).ToList()))
                .ForMember(dest => dest.LeadName,
                    opt => opt.MapFrom(src => src.Lead != null ? src.Lead.Name : null))
                .ForMember(dest => dest.CreatedByAdminName,
                    opt => opt.MapFrom(src => src.CreatedByAdmin != null ? src.CreatedByAdmin.Name : null));
            //CreateMap<Team, TeamGetDTO>()
            //    .ForMember(
            //        dest => dest.TeamDeveloperIDs,
            //        opt => opt.MapFrom(src => src.TeamDevelopers.Select(td => td.DeveloperId).ToList())
            //    )
            //    .ForMember(
            //        dest => dest.TeamTopicIDs,
            //        opt => opt.MapFrom(src => src.TeamTopics.Select(tt => tt.TopicId).ToList())
            //    );




        }
        }
}

