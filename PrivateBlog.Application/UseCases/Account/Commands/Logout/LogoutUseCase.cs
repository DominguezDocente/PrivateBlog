using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Commands.Logout
{
    public class LogoutUseCase : IRequestHandler<LogoutCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public LogoutUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Handle(LogoutCommand request)
        {
            await _accountRepository.SignOutAsync();
        }
    }
}
