using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetBlogById
{
    public class GetAccessibleBlogByIdQuery : IRequest<AccessibleBlogDetailDTO>
    {
        public required string UserId { get; set; }
        public required Guid BlogId { get; set; }
    }
}
