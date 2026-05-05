using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo
{
    public class GetAccountUserInfoQuery : IRequest<UserAccountInfoDTO>
    {
        public required string UserId { get; set; }
    }
}
