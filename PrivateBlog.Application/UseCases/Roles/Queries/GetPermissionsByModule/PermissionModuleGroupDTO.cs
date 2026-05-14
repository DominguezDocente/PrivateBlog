using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule
{
    public class PermissionModuleGroupDTO
    {
        public required string Module { get; set; }
        public required List<PermissionItemDTO> Permissions { get; set; }
    }
}
