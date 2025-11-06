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

            CreateMap<User, AccountUserDTO>().ReverseMap();

            CreateMap<Permission, PermissionDTO>();

            CreateMap<PrivateBlogRole, PrivateBlogRoleDTO>().ReverseMap();
        }
    }
}
