using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetUsersList
{
    public class UserListItemDTO
    {
        public required string Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string RoleName { get; init; }
    }
}
