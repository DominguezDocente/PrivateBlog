using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.DTOs
{
    public class SectionDTO : Section
    {
        public PaginationResponse<Blog> PaginatedBlogs { get; set; }
    }
}
