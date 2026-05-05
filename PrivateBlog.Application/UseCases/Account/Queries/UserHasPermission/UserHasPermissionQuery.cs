using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.UserHasPermission
{
    public class UserHasPermissionQuery : IRequest<bool>
    {
        public required string UserId { get; set; }
        public required string PermissionCode { get; set; }
    }
}
