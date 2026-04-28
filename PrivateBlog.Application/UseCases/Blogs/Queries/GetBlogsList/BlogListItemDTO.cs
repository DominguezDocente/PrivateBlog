using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList
{
    public sealed class BlogListItemDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = null!;

        public Guid SectionId { get; init; }

        public string SectionName { get; init; } = null!;

        public bool IsPublished { get; init; }

        public DateTime CreatedAt { get; init; }
    }
}
