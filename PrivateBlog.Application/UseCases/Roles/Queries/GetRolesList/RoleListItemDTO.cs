namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList
{
    public sealed class RoleListItemDTO
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required int PermissionCount { get; init; }
    }
}
