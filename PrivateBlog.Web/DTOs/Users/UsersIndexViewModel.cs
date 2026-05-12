using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Users.Queries.GetRoleOptions;
using PrivateBlog.Application.UseCases.Users.Queries.GetUsersList;

namespace PrivateBlog.Web.DTOs.Users
{
    public class UsersIndexViewModel
    {
        public required PaginationResponse<UserListItemDTO> List { get; set; }
        public string FilterName { get; set; } = string.Empty;
        public Guid? FilterRoleId { get; set; }
        public IReadOnlyList<RoleOptionDTO> Roles { get; set; } = [];
    }
}
