using System.ComponentModel.DataAnnotations;
using pbSection = PrivateBlog.Web.Data.Entities.Section;

namespace PrivateBlog.Web.DTOs.Blog
{
    public class BlogDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Guid SectionId { get; set; }

        public pbSection? Section { get; set; }
    }
}
