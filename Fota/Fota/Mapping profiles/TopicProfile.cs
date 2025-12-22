using AutoMapper;
using Fota.DataLayer.Models;
using Fota.Models;
using SharedProjectDTOs.Topics;

namespace Fota.Mapping_profiles
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<Topic, TopicGetDTO>()
                        .ForMember(
                            dest => dest.TopicSubscribersIDs,
                            opt => opt.MapFrom(src => src.TopicSubscribers.Select(d => d.Id).ToList())
                        )
                        .ForMember(
                            dest => dest.MessagesIDs,
                            opt => opt.MapFrom(src => src.Messages.Select(d => d.Id).ToList())
                        )
                        .ForMember(
                            dest => dest.DiagnosticsIDs,
                            opt => opt.MapFrom(src => src.Diagnostics.Select(t => t.Id).ToList())
                        )
                        .ForMember(
                            dest => dest.TeamTopicsIDs,
                            opt => opt.MapFrom(src => src.TeamTopics.Select(t => t.Id).ToList())
                        );
        }
    }
}