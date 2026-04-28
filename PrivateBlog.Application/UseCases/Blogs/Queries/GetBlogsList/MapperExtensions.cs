using PrivateBlog.Domain.Entities.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

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
                CreatedAt = blog.CreatedAt,
            };
        }
    }
}
