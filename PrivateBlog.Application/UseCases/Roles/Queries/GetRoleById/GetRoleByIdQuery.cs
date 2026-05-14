using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRoleById
{
    public sealed class GetRoleByIdQuery : IRequest<RoleDetailDTO>
    {
        public required Guid Id { get; init; }
    }
}
