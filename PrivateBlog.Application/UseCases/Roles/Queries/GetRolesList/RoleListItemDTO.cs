using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList
{
    public class RoleListItemDTO
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required List<Guid> PermissionIds { get; init; }
        public int PermissionCount { get; set; }
    }
}
