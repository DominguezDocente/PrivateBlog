using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Services.Abtractions
{
    public interface IBlogsService
    {
        public Task<Response<BlogDTO>> CreateAsync(BlogDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<BlogDTO>> EditAsync(BlogDTO dto);
        public Task<Response<BlogDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<BlogDTO>>> GetPaginatedListAsync(PaginationRequest request);
    }
}
