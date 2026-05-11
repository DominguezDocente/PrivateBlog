using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
        public List<Guid> PermissionIds { get; set; } = [];
    }
}
