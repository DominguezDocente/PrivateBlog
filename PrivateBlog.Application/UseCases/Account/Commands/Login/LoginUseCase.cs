using PrivateBlog.Application.Contracts.Authentication;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Commands.Login
{
    public sealed class LoginUseCase : IRequestHandler<LoginCommand, AccountSignInResult>
    {
        private readonly IAccountService _accountService;

        public LoginUseCase(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public Task<AccountSignInResult> Handle(LoginCommand request)
        {
            return _accountService.SignInWithPasswordAsync(
                request.Email.Trim(),
                request.Password,
                request.RememberMe,
                CancellationToken.None);
        }
    }
}
