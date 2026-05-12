using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetRoleOptions
{
    public class GetRoleOptionsQuery : IRequest<IReadOnlyList<RoleOptionDTO>>
    {
    }
}
