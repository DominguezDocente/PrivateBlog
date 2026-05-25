using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Commands.ChangePassword
{
    public class ChangePasswordUseCase : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public ChangePasswordUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Handle(ChangePasswordCommand request)
        {
            await _accountRepository.ChangePasswordAsync(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword);
        }
    }
}
