using AutoMapper;
using Fota.Models;
using SharedProjectDTOs.Subscribers;

namespace Fota.Mapping_profiles
{
    public class SubscriberMappingProfile : Profile
    {
        public SubscriberMappingProfile()
        {
            // Subscriber -> SubscriberGetDto
            CreateMap<Subscriber, SubscriberGetDto>()
                .ForMember(dest => dest.TopicSubscriptionIds, opt => opt.MapFrom(src => src.TopicSubscriptions.Select(ts => ts.Id).ToList()))
                .ForMember(dest => dest.DiagnosticIds, opt => opt.MapFrom(src => src.Diagnostics.Select(d => d.Id).ToList()))
                .ForMember(dest => dest.MessageDeliveryIds, opt => opt.MapFrom(src => src.MessageDeliveries.Select(md => md.Id).ToList()));

            // Subscriber -> SubscriberTopicDto
            CreateMap<Subscriber, SubscriberTopicDto>();

            // SubscriberCreateDto -> Subscriber
            CreateMap<SubscriberCreateDto, Subscriber>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}