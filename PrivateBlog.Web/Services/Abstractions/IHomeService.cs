using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Blog;
using PrivateBlog.Web.DTOs.Section;

namespace PrivateBlog.Web.Services.Abstractions
{
    public interface IHomeService
    {
        public Task<Response<PaginationResponse<SectionDTO>>> GetSectionsAsync(PaginationRequest request);
        public Task<Response<SectionDTO>> GetSectionAsync(Guid id, PaginationRequest request);
        public Task<Response<BlogDTO>> GetBlogAsync(Guid id);
    }
}
