namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogById
{
    public sealed class BlogDetailDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = null!;

        public string Content { get; init; } = null!;

        public Guid SectionId { get; init; }

        public string SectionName { get; init; } = null!;

        public bool IsPublished { get; init; }

        public DateTime CreatedAtUtc { get; init; }

        public DateTime UpdatedAtUtc { get; init; }
    }
}
