using AutoMapper;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Section;

namespace PrivateBlog.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Section, SectionDTO>().ForMember(dto => dto.Name, entity => entity.MapFrom(s => s.Name))
                                            .ReverseMap();
        }
    }
}
