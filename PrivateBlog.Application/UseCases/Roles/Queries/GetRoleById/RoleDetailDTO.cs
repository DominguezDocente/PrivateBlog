namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRoleById
{
    public sealed class RoleDetailDTO
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required List<Guid> PermissionIds { get; init; }
    }
}
