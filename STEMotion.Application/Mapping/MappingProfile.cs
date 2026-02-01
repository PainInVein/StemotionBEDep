using AutoMapper;
using PayOS.Models.V2.PaymentRequests;
using PayOS.Models.Webhooks;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Extensions;
using STEMotion.Domain.Entities;

namespace STEMotion.Application.Middleware
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDTO>()
                    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                    .ReverseMap();

            CreateMap<CreateUserRequestDTO, User>().ReverseMap();

            CreateMap<UpdateUserRequestDTO, User>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Grade, GradeResponseDTO>().ReverseMap();
            CreateMap<GradeRequestDTO, Grade>().ReverseMap();
            CreateMap<UpdateGradeRequest, Grade>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Subject, SubjectResponseDTO>()
                .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.Grade.GradeLevel))
                .ReverseMap();
            CreateMap<SubjectRequestDTO, Subject>().ReverseMap();
            CreateMap<UpdateSubjectRequestDTO, Subject>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForMember(dest => dest.GradeLevel, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Chapter, ChapterResponseDTO>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
                .ReverseMap();

            CreateMap<ChapterRequestDTO, Chapter>().IgnoreAllNonExisting().ReverseMap();
            CreateMap<UpdateChapterRequestDTO, Chapter>().IgnoreAllNonExisting().ReverseMap();

            CreateMap<Lesson, LessonResponseDTO>()
                .ForMember(dest => dest.ChapterName, opt => opt.MapFrom(src => src.Chapter.ChapterName))
                .IgnoreAllNonExisting()
                .ReverseMap();

            CreateMap<LessonRequestDTO, Lesson>().IgnoreAllNonExisting().ReverseMap();
            CreateMap<UpdateLessonRequestDTO, Lesson>().IgnoreAllNonExisting().ReverseMap();

            CreateMap<GoogleRequestDTO, User>()
                .IgnoreAllNonExisting()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Subscription, SubscriptionResponseDTO>().ReverseMap();

            CreateMap<CreatePaymentLinkResponse, PaymentResponseDTO>()
                .ForMember(dest => dest.CheckoutUrl, opt => opt.MapFrom(src => src.CheckoutUrl))
                .ForMember(dest => dest.PaymentLinkId, opt => opt.MapFrom(src => src.PaymentLinkId))
                .ForMember(dest => dest.OrderCode, opt => opt.MapFrom(src => src.OrderCode))
                .ReverseMap();

            CreateMap<WebhookData, SubscriptionPayment>()
            .ForMember(dest => dest.CounterAccountBankId, opt => opt.MapFrom(src => src.CounterAccountBankId))
            .ForMember(dest => dest.CounterAccountName, opt => opt.MapFrom(src => src.CounterAccountName))
            .ForMember(dest => dest.CounterAccountNumber, opt => opt.MapFrom(src => src.CounterAccountNumber));

        }
    }
}
