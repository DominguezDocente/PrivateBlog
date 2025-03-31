using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Services
{
    public interface IBlogsService
    {
        Task<Response<BlogDTO>> CreateAsync(BlogDTO dto);
        Task<Response<BlogDTO>> EditAsync(BlogDTO dto);
        Task<Response<BlogDTO>> GetOneAsync(int id);
        Task<Response<PaginationResponse<SectionDTO>>> GetPaginationAsync(PaginationRequest request);
    }
    public class BlogsService : IBlogsService
    {
    }
}
