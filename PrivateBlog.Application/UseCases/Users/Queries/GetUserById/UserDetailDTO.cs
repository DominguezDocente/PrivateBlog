namespace PrivateBlog.Application.UseCases.Users.Queries.GetUserById
{
    public sealed class UserDetailDTO
    {
        public required string Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string? PhoneNumber { get; init; }
        public required Guid RoleId { get; init; }
    }
}
