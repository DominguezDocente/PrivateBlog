using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogById
{
    public sealed class GetBlogByIdQuery : IRequest<BlogDetailDTO?>
    {
        public Guid Id { get; init; }
    }
}
