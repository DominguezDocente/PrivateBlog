using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetRoleOptions
{
    public class RoleOptionDTO
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}
