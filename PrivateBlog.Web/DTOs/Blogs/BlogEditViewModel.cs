using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions;

namespace PrivateBlog.Web.DTOs.Blogs
{
    public class BlogEditViewModel
    {
        public EditBlogDTO Blog { get; set; } = new();

        public IReadOnlyList<SectionOptionDTO> SectionOptions { get; set; } = Array.Empty<SectionOptionDTO>();
    }
}
