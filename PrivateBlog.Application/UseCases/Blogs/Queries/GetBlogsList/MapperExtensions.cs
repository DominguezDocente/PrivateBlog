using PrivateBlog.Domain.Entities.Blogs;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList
{
    internal static class MapperExtensions
    {
        public static BlogListItemDTO ToListItemDTO(this Blog blog)
        {
            return new BlogListItemDTO
            {
                Id = blog.Id,
                Name = blog.Name,
                SectionId = blog.SectionId,
                SectionName = blog.Section.Name,
                IsPublished = blog.IsPublished,
                UpdatedAtUtc = blog.UpdatedAtUtc,
            };
        }
    }
}
