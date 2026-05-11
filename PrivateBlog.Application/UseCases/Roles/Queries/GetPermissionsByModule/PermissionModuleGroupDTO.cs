using PrivateBlog.Domain.Entities.Account;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule
{
    public sealed class PermissionModuleGroupDTO
    {
        public required PermissionModule Module { get; init; }
        public required List<PermissionItemDTO> Permissions { get; init; }
    }
}
