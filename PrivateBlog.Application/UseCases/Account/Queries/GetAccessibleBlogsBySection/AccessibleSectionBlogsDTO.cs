namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogsBySection
{
    public class AccessibleSectionBlogsDTO
    {
        public Guid SectionId { get; init; }
        public string SectionName { get; init; } = string.Empty;
        public IReadOnlyList<AccessibleBlogListItemDTO> Blogs { get; init; } = Array.Empty<AccessibleBlogListItemDTO>();
    }
}
