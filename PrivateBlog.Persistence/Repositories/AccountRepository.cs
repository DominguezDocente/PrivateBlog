using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using Microsoft.AspNetCore.Identity;
using PrivateBlog.Persistence.Entitities;


namespace PrivateBlog.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRepository(SignInManager<ApplicationUser> signinManager, UserManager<ApplicationUser> userManager)
        {
            _signinManager = signinManager;
            _userManager = userManager;
        }

        public async Task<AccountSignInResult> SignInAsync(string userName, string password, bool rememberMe, CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);

            if (user is null) 
            {
                return new AccountSignInResult
                {
                    Succeeded = false,
                    IsLockedOut = false
                };
            }

            SignInResult result = await _signinManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: true);

            return new AccountSignInResult
            {
                Succeeded = result.Succeeded,
                IsLockedOut = result.IsLockedOut
            };
        }

        public Task SignOutAsync(CancellationToken cancellationToken = default)
        {
            return _signinManager.SignOutAsync();
        }
    }
}
