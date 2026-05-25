namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleSections
{
    public class AccessibleSectionItemDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int PublishedBlogsCount { get; init; }
    }
}
