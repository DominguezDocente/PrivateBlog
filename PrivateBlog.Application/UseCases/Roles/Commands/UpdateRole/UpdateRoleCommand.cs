using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Commands.UpdateRole
{
    public sealed class UpdateRoleCommand : IRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public List<Guid> PermissionIds { get; set; } = [];
    }
}
