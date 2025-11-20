using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Services.Abtractions
{
    public interface IHomeService
    {
        public Task<Response<BlogDTO>> GetBlogAsync(Guid id);
        public Task<Response<SectionDTO>> GetSectionAsync(Guid id, PaginationRequest request);
        public Task<Response<PaginationResponse<SectionDTO>>> GetSectionsAsync(PaginationRequest request);
    }
}
