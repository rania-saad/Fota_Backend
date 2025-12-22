

//using AutoMapper;
//using Fota.Models;
//using SharedProjectDTOs.BaseMessages;

//namespace StaffAffairs.AWebAPI.Mappings
//{
//    public class BaseMessageProfile : Profile
//    {
//        public BaseMessageProfile()
//        {
//            // BaseMessage -> BaseMessageGetDto
//            CreateMap<BaseMessage, BaseMessageGetDto>()
//                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic != null ? src.Topic.Name : null))
//                .ForMember(dest => dest.UploaderName, opt => opt.MapFrom(src => src.Uploader != null ? src.Uploader.Name : null))
//                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher != null ? src.Publisher.Name : null))
//                .ForMember(dest => dest.DeliveryIds, opt => opt.MapFrom(src => src.Deliveries.Select(d => d.Id).ToList()));

//            // BaseMessage -> BaseMessageListDto
//            CreateMap<BaseMessage, BaseMessageListDto>()
//                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic != null ? src.Topic.Name : null))
//                .ForMember(dest => dest.UploaderName, opt => opt.MapFrom(src => src.Uploader != null ? src.Uploader.Name : null));

//            // BaseMessage -> BaseMessageWithFileDto
//            CreateMap<BaseMessage, BaseMessageWithFileDto>();

//            // BaseMessageCreateDto -> BaseMessage
//            CreateMap<BaseMessageCreateDto, BaseMessage>()
//                .ForMember(dest => dest.Id, opt => opt.Ignore())
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Draft"))
//                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
//                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
//        }
//    }
//}

using AutoMapper;
using Fota.Models;
using SharedProjectDTOs.BaseMessages;

namespace Fota.Mapping_profiles
{
    public class BaseMessageProfile : Profile
    {
        public BaseMessageProfile()
        {
            // ✅ BaseMessage → BaseMessageGetDto (زي DeveloperGetDTO)
            CreateMap<BaseMessage, BaseMessageGetDto>()
                .ForMember(
                    dest => dest.TopicName,
                    opt => opt.MapFrom(src => src.Topic.Name)
                )
                .ForMember(
                    dest => dest.UploaderName,
                    opt => opt.MapFrom(src => src.Uploader.Name)
                )
                .ForMember(
                    dest => dest.PublisherName,
                    opt => opt.MapFrom(src => src.Publisher.Name)
                )
                .ForMember(
                    dest => dest.DeliveryIds,
                    opt => opt.MapFrom(src => src.Deliveries.Select(d => d.Id).ToList())
                );

            // ✅ BaseMessage → BaseMessageListDto (عرض مختصر زي DeveloperTeamDto)
            CreateMap<BaseMessage, BaseMessageListDto>()
                .ForMember(
                    dest => dest.TopicName,
                    opt => opt.MapFrom(src => src.Topic.Name)
                )
                .ForMember(
                    dest => dest.UploaderName,
                    opt => opt.MapFrom(src => src.Uploader.Name)
                );

            // ✅ BaseMessage → DTO يحتوي على الملف (بدون تعقيد)
            CreateMap<BaseMessage, BaseMessageWithFileDto>();

            // ✅ Create DTO → Entity (لو احتجتيه)
            CreateMap<BaseMessageCreateDto, BaseMessage>().ReverseMap();

            // ✅ Update DTO → Entity (لو احتجتيه)
            CreateMap<BaseMessageUpdateDto, BaseMessage>().ReverseMap();

            // ✅ Approve DTO → Entity (لو احتجتيه)
            CreateMap<BaseMessageApproveDto, BaseMessage>().ReverseMap();

            // ✅ Reject DTO → Entity (لو احتجتيه)
            CreateMap<BaseMessageRejectDto, BaseMessage>().ReverseMap();

            // ✅ Publish DTO → Entity (لو احتجتيه)
            CreateMap<BaseMessagePublishDto, BaseMessage>().ReverseMap();
        }
    }
}
