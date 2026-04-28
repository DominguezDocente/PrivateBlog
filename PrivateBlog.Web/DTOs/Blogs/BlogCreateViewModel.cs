using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions;

namespace PrivateBlog.Web.DTOs.Blogs
{
    public class BlogCreateViewModel
    {
        public CreateBlogDTO Blog { get; set; } = new();

        public IReadOnlyList<SectionOptionDTO> SectionOptions { get; set; } = Array.Empty<SectionOptionDTO>();
    }
}
