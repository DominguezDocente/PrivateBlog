using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.UserHasPermission
{
    public sealed class UserHasPermissionQuery : IRequest<bool>
    {
        public required string UserId { get; init; }

        public required string PermissionCode { get; init; }
    }
}
