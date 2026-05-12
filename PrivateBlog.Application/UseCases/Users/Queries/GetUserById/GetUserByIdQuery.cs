using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetUserById
{
    public sealed class GetUserByIdQuery : IRequest<UserDetailDTO>
    {
        public required string Id { get; init; }
    }
}
