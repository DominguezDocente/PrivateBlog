using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions;

namespace PrivateBlog.Web.DTOs.Blogs
{
    public class BlogsIndexViewModel
    {
        public required PaginationResponse<BlogListItemDTO> List { get; init; }

        public string FilterName { get; init; } = string.Empty;

        public Guid? FilterSectionId { get; init; }

        public bool? FilterIsPublished { get; init; }

        public IReadOnlyList<SectionOptionDTO> SectionOptions { get; init; } = Array.Empty<SectionOptionDTO>();
    }
}
