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

            CreateMap<PrivateBlogRoleDTO, PrivateBlogRole>().ReverseMap();

            CreateMap<Permission, PermissionDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<User, AccountUserDTO>().ForMember(x => x.Photo, options => options.Ignore())
                                             .ForMember(u => u.PhotoUrl, config => config.MapFrom(dto => dto.Photo));

            CreateMap<AccountUserDTO, User>().ForMember(u => u.UserName, config => config.MapFrom(dto => dto.Email))
                                             .ForMember(x => x.Photo, options => options.Ignore());
        }
    }
}
