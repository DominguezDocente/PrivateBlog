using PrivateBlog.Application.UseCases.Account.Commands.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IAccountRepository
    {
        Task<AccountSignInResult> SignInAsync(string userName, string password, bool rememberMe, CancellationToken cancellationToken = default);

        Task SignOutAsync(CancellationToken cancellationToken = default);
    }
}
