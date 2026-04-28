using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList
{
    public sealed class GetBlogsListQuery : IRequest<PaginationResponse<BlogListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Normalized();

        public string? NameFilter { get; set; }

        public Guid? SectionIdFilter { get; set; }

        public bool? IsPublishedFilter { get; set; }
    }
}
