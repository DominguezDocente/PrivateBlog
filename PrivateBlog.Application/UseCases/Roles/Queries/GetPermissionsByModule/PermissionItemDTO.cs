using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule
{
    public class PermissionItemDTO
    {
        public required Guid Id { get; init; }
        public required string Code { get; init; }
        public required string Description { get; init; }
    }
}
