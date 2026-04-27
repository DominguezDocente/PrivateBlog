using PrivateBlog.Application.Contracts.Authentication;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Commands.Login
{
    public sealed class LoginCommand : IRequest<AccountSignInResult>
    {
        public required string Email { get; init; }

        public required string Password { get; init; }

        public bool RememberMe { get; init; }
    }
}
