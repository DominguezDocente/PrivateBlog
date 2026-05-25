using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<AccountProfileDTO>
    {
        public required string UserId { get; set; }
    }
}
