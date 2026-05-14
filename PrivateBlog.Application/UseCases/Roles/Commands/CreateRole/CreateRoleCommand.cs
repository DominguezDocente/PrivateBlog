using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
        public List<Guid> PermissionIds { get; set; } = [];
    }
}
