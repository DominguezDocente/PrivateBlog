using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo
{
    public sealed class UserAccountInfoDTO
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string RoleName { get; init; }
        public string FullName => $"{FirstName} {LastName}";

    }
}
