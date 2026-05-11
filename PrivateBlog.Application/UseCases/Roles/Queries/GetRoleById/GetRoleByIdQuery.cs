using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRoleById
{
    public sealed class GetRoleByIdQuery : IRequest<RoleDetailDTO>
    {
        public required Guid Id { get; init; }
    }
}
