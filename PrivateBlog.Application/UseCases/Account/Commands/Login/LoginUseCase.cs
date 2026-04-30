using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Commands.Login
{
    public class LoginUseCase : IRequestHandler<LoginCommand, AccountSignInResult>
    {
        private readonly IAccountRepository _accountRepository;

        public LoginUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<AccountSignInResult> Handle(LoginCommand request)
        {
            return _accountRepository.SignInAsync(request.UserName, request.Password, request.RememberMe);
        }
    }
}
