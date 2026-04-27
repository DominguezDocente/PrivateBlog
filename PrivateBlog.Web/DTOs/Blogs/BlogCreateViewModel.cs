using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionOptions;

namespace PrivateBlog.Web.DTOs.Blogs
{
    public sealed class BlogCreateViewModel
    {
        public CreateBlogDTO Blog { get; set; } = new();

        public IReadOnlyList<SectionOptionDTO> SectionOptions { get; set; } = Array.Empty<SectionOptionDTO>();
    }
}
