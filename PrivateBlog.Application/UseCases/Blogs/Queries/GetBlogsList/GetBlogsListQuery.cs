using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList
{
    public sealed class GetBlogsListQuery : IRequest<PaginationResponse<BlogListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = new();

        public string? NameFilter { get; set; }

        public Guid? SectionIdFilter { get; set; }

        /// <summary>null = todas, true/false = filtro por publicación.</summary>
        public bool? IsPublishedFilter { get; set; }
    }
}
