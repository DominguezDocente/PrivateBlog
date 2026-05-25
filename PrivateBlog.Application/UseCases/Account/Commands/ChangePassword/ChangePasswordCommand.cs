using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        public required string UserId { get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
