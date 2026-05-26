using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetBlogsBySection
{
    public class GetAccessibleBlogsBySectionQuery : IRequest<AccessibleSectionBlogsDTO>
    {
        public required string UserId { get; set; }
        public required Guid SectionId { get; set; }
    }
}
