namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogsBySection
{
    public class AccessibleBlogListItemDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
