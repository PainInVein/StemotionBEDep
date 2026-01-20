using AutoMapper;
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

            CreateMap<Subject, SubjectResponseDTO>().ReverseMap();
            CreateMap<SubjectRequestDTO, Subject>().ReverseMap();
            CreateMap<UpdateSubjectRequestDTO, Subject>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Chapter, ChapterResponseDTO>().ReverseMap();

            CreateMap<ChapterRequestDTO, Chapter>().IgnoreAllNonExisting().ReverseMap();
            CreateMap<UpdateChapterRequestDTO, Chapter>().IgnoreAllNonExisting().ReverseMap();

            CreateMap<Lesson, LessonResponseDTO>().ReverseMap();

            CreateMap<LessonRequestDTO, Lesson>().IgnoreAllNonExisting().ReverseMap();
            CreateMap<UpdateLessonRequestDTO, Lesson>().IgnoreAllNonExisting().ReverseMap();
        }
    }
}
