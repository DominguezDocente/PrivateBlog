using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Blog;
using PrivateBlog.Web.DTOs.Role;

namespace PrivateBlog.Web.Services.Abstractions
{
    public interface IRolesService
    {
        public Task<Response<RoleDTO>> CreateAsync(RoleDTO dto);
        public Task<Response> DeleteAsync(Guid id);
        public Task<Response<RoleDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<RoleDTO>>> GetPaginationAsync(PaginationRequest request);
        public Task<Response<RoleDTO>> UpdateAsync(RoleDTO dto);
        public Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync();
        public Task<Response<List<SectionsForRoleDTO>>> GetSectionsAsync();
    }
}
