using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.UpdateBlog
{
    public sealed class UpdateBlogCommand : IRequest
    {
        public Guid Id { get; init; }

        public required string Name { get; init; }

        public required string Content { get; init; }

        public Guid SectionId { get; init; }

        public bool IsPublished { get; init; }
    }
}
