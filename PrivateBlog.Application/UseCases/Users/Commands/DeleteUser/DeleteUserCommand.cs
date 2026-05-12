using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Commands.DeleteUser
{
    public sealed class DeleteUserCommand : IRequest
    {
        public required string Id { get; set; }
    }
}
