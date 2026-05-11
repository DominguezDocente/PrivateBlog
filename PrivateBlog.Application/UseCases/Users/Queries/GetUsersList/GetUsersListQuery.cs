using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetUsersList
{
    public sealed class GetUsersListQuery : IRequest<PaginationResponse<UserListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Normalized();
        public string? NameFilter { get; set; }
        public Guid? RoleIdFilter { get; set; }
    }
}
