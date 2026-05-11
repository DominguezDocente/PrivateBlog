namespace PrivateBlog.Application.UseCases.Users.Queries.GetUsersList
{
    public sealed class UserListItemDTO
    {
        public required string Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string RoleName { get; init; }
    }
}
