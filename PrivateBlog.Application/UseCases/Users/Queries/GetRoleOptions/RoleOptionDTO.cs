namespace PrivateBlog.Application.UseCases.Users.Queries.GetRoleOptions
{
    public sealed class RoleOptionDTO
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
