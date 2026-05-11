using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule
{
    public sealed class GetPermissionsByModuleQuery : IRequest<IReadOnlyList<PermissionModuleGroupDTO>>
    {
    }
}
