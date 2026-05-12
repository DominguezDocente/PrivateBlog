using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Users.Commands.CreateUser
{
    public sealed class CreateUserCommand : IRequest<string>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public required Guid RoleId { get; set; }
    }
}
