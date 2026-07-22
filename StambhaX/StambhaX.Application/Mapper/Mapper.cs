using AutoMapper;
using StambhaX.Application.DTOs;
using StambhaX.Core.Entities;

namespace StambhaX.Application.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            // Mapping configurations
            // User to UserDto mapping
            CreateMap<User, UserDto>().ForMember(dest=> dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role!.Name).ToList())).ReverseMap();

            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
