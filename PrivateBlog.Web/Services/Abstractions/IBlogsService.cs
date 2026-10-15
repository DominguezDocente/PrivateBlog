using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Blog;

namespace PrivateBlog.Web.Services.Abstractions
{
    public interface IBlogsService
    {
        public Task<Response<CreateBlogDTO>> CreateAsync(CreateBlogDTO dto);
        public Task<Response> DeleteAsync(Guid id);
        public Task<Response<BlogDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<BlogDTO>>> GetPaginationAsync(PaginationRequest request);
        public Task<Response<BlogDTO>> UpdateAsync(UpdateBlogDTO dto);
    }
}
