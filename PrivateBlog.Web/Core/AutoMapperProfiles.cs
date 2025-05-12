using AutoMapper;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Section, SectionDTO>().ReverseMap();

            CreateMap<Blog, BlogDTO>().ReverseMap();

            CreateMap<PrivateBlogRole, PrivateBlogRoleDTO>().ReverseMap();

            CreateMap<Permission, PermissionDTO>();

            CreateMap<User, UserDTO>();

            CreateMap<UserDTO, User>().ForMember(user => user.UserName, config => config.MapFrom(dto => dto.Email));

            CreateMap<User, AccountUserDTO>().ForMember(dest => dest.Photo, options => options.Ignore())
                                             .ForMember(dest => dest.PhotoUrl, config => config.MapFrom(src => src.Photo));

            CreateMap<AccountUserDTO, User>().ForMember(dest => dest.Photo, options => options.Ignore())
                                             .ForMember(user => user.UserName, config => config.MapFrom(dto => dto.Email));


        }
    }
}
