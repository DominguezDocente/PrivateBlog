using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Roles.Commands.UpdateRole
{
    public sealed class UpdateRoleCommand : IRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public List<Guid> PermissionIds { get; set; } = [];
    }
}
