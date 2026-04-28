using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.DeleteBlog
{
    public sealed class DeleteBlogCommand : IRequest
    {
        public Guid Id { get; init; }
    }
}
