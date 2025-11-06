using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.DTOs;

namespace PrivateRole.Web.Services.Abtractions
{
    public interface IRolesService
    {
        public Task<Response<PrivateBlogRoleDTO>> CreateAsync(PrivateBlogRoleDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<PrivateBlogRoleDTO>> EditAsync(PrivateBlogRoleDTO dto);
        public Task<Response<PrivateBlogRoleDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<PrivateBlogRoleDTO>>> GetPaginatedListAsync(PaginationRequest request);
        public Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync();
    }
}
