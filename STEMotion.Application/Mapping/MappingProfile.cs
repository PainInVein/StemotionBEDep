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
        }
    }
}
