using PrivateBlog.Application.Contracts.Authentication;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Commands.Logout
{
    public sealed class LogoutUseCase : IRequestHandler<LogoutCommand>
    {
        private readonly IAccountService _accountService;

        public LogoutUseCase(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Handle(LogoutCommand request)
        {
            await _accountService.SignOutAsync(CancellationToken.None);
        }
    }
}
