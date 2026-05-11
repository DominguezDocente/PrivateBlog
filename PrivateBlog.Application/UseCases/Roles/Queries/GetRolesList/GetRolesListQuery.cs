using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList
{
    public sealed class GetRolesListQuery : IRequest<PaginationResponse<RoleListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Normalized();
        public string? NameFilter { get; set; }
    }
}
