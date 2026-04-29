namespace PrivateBlog.Application.Contracts.Account
{
    public sealed class UserHeaderInfoDTO
    {
        public required string FirstName { get; init; }

        public required string LastName { get; init; }

        public required string FullName { get; init; }

        public required string RoleName { get; init; }
    }
}
