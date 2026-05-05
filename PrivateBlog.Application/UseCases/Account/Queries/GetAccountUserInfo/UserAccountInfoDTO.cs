using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo
{
    public class UserAccountInfoDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string RoleName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
