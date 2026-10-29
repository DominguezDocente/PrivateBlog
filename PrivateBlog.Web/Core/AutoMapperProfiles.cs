using AutoMapper;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.DTOs.Blog;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.DTOs.User;

namespace PrivateBlog.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Section, CreateSectionDTO>().ReverseMap();
            CreateMap<Section, UpdateSectionDTO>().ReverseMap();
            CreateMap<Section, SectionDTO>().ForMember(dto => dto.Name, entity => entity.MapFrom(s => s.Name))
                                            .ReverseMap();

            CreateMap<Blog, CreateBlogDTO>().ReverseMap();
            CreateMap<Blog, UpdateBlogDTO>().ReverseMap();
            CreateMap<Blog, BlogDTO>().ReverseMap();

            CreateMap<User, AccountUserDTO>().ForMember(dto => dto.Phone, entity => entity.MapFrom(e => e.PhoneNumber))
                                             .ForMember(dto => dto.Id, entity => entity.MapFrom(e => Guid.Parse(e.Id)))
                                             .ForMember(dto => dto.RoleName, entity => entity.MapFrom(e => e.PrivateBlogRole.Name));

            CreateMap<User, UserDTO>().ForMember(dto => dto.Phone, entity => entity.MapFrom(e => e.PhoneNumber))
                                      .ForMember(dto => dto.Id, entity => entity.MapFrom(e => Guid.Parse(e.Id)));

            CreateMap<User, CreateUserDTO>().ForMember(dto => dto.Phone, entity => entity.MapFrom(e => e.PhoneNumber))
                                            .ReverseMap();                                        

            CreateMap<User, UpdateUserDTO>().ForMember(dto => dto.Phone, entity => entity.MapFrom(e => e.PhoneNumber))
                                            .ForMember(dto => dto.Id, entity => entity.MapFrom(e => Guid.Parse(e.Id)))
                                            .ReverseMap();
        }
    }
}
