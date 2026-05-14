using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule
{
    public class GetPermissionsByModuleQuery : IRequest<IReadOnlyList<PermissionModuleGroupDTO>>
    {
    }
}
