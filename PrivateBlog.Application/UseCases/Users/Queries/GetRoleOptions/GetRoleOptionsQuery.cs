using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetRoleOptions
{
    public sealed class GetRoleOptionsQuery : IRequest<IReadOnlyList<RoleOptionDTO>>
    {
    }
}
