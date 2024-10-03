using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Helpers
{
    public interface IConverterHelper
    {
        public Blog ToBlog(BlogDTO dto);
    }

    public class ConverterHelper : IConverterHelper
    {
        public Blog ToBlog(BlogDTO dto)
        {
            return new Blog
            {
                Content = dto.Content,
                Id = dto.Id,
                IsPublished = dto.IsPublished,
                SectionId = dto.SectionId,
                Title = dto.Title,
            };
        }
    }
}
