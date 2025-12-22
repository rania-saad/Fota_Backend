using AutoMapper;
using Fota.Models;
using Fota.Models.DTOs;

namespace Fota.MappingProfiles
{
    public class MessageGetProfile : Profile
    {
        public MessageGetProfile()
        {
            // Map from Entity → DTO (لـ GET Methods)
            CreateMap<BaseMessage, MessageGetDTO>()
                .ForMember(dest => dest.TopicId, opt => opt.MapFrom(src => src.TopicId))
                .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.PublisherId));

            // Map from DTO → Entity (لـ POST / Update Methods)
            CreateMap<MessageGetDTO, BaseMessage>()
                .ForMember(dest => dest.Topic, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherId, opt => opt.Ignore());
        }


    }

    public class MessagePostProfile : Profile
    {
        public MessagePostProfile()
        {
            //// Map from Entity → DTO (لـ GET Methods)
            //CreateMap<Message, MessagePostDTO>()
            //    .ForMember(dest => dest.TopicId, opt => opt.MapFrom(src => src.TopicId))
            //    .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.PublisherId));

            //// Map from DTO → Entity (لـ POST / Update Methods)
            //CreateMap<MessagePostDTO, Message>()
            //    .ForMember(dest => dest.Topic, opt => opt.Ignore())
            //    .ForMember(dest => dest.Publisher, opt => opt.Ignore());


            CreateMap<MessagePostDTO, BaseMessage>();

            // من Entity → DTO
            CreateMap<BaseMessage, MessagePostDTO>()
                .ForMember(dest => dest.TopicName,
                           opt => opt.MapFrom(src => src.Topic.Name));
        }


    }

    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            //CreateMap<TopicDTO, Topic>()
            //    .ForMember(dest => dest.TopicId, opt => opt.Ignore());
            //CreateMap<Topic, TopicDTO>();

            CreateMap<TopicDTO, Topic>();   // AutoMapper مش هيلاقي TopicId في الـ DTO → هيسيبها default
            CreateMap<Topic, TopicDTO>();
        }
    }

    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<SubscriberDTO, Subscriber>();   // AutoMapper مش هيلاقي TopicId في الـ DTO → هيسيبها default
            CreateMap<Subscriber, SubscriberDTO>();

        }
    }

//    public class PublisherFetProfile : Profile 
//    {
//        public PublisherFetProfile()
//        {
//            CreateMap<Publisher, PublisherGetDTO>();


//            // Map from DTO → Entity (لـ POST / Update Methods)
//            CreateMap<PublisherGetDTO, Publisher>()
//                .ForMember(dest => dest.Messages, opt => opt.Ignore());
//        }
//    }

//    public class PublisherPostProfile : Profile 
//    {
//        public PublisherPostProfile()
//        {
//            CreateMap<Publisher, PublisherPostDTO>();


//            // Map from DTO → Entity (لـ POST / Update Methods)
//            CreateMap<PublisherPostDTO, Publisher>()
//                .ForMember(dest => dest.Messages, opt => opt.Ignore());
//        }
//    }
}
