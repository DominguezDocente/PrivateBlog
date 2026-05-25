using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Commands.ChangePassword
{
    public class ChangePasswordUseCase : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public ChangePasswordUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Handle(ChangePasswordCommand command)
        {
            await _accountRepository.ChangePasswordAsync(
                command.UserId,
                command.CurrentPassword,
                command.NewPassword);
        }
    }
}
