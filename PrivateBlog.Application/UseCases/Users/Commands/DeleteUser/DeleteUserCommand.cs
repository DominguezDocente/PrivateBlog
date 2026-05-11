using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Users.Commands.DeleteUser
{
    public sealed class DeleteUserCommand : IRequest
    {
        public required string Id { get; set; }
    }
}
