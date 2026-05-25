namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogById
{
    public class AccessibleBlogDetailDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public Guid SectionId { get; init; }
        public string SectionName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
