using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Commands.Login
{
    public class AccountSignInResult
    {
        public required bool Succeeded { get; set; }
        public required bool IsLockedOut { get; set; }

        public bool InvalidCredentials => !Succeeded && !IsLockedOut;
    }
}
