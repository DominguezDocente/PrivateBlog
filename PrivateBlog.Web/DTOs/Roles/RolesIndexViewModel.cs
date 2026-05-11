using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList;

namespace PrivateBlog.Web.DTOs.Roles
{
    public sealed class RolesIndexViewModel
    {
        public required PaginationResponse<RoleListItemDTO> List { get; set; }
        public string FilterName { get; set; } = string.Empty;
    }
}
