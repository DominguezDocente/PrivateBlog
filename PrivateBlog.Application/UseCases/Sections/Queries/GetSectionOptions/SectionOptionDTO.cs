namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionOptions
{
    public sealed class SectionOptionDTO
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = null!;
    }
}
