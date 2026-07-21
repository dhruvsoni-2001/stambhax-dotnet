using AutoMapper;
using StambhaX.Api.DTOs;
using StambhaX.Api.Models;

namespace StambhaX.Api.Mapper
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
