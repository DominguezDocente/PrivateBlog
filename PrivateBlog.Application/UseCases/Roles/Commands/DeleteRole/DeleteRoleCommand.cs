using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Roles.Commands.DeleteRole
{
    public sealed class DeleteRoleCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}
