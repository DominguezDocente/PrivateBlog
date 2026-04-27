using Microsoft.AspNetCore.Identity;
using PrivateBlog.Application.Contracts.Authentication;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Infrastructure.Authentication
{
    public sealed class IdentityAccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityAccountService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<AccountSignInResult> SignInWithPasswordAsync(
            string email,
            string password,
            bool rememberMe,
            CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new AccountSignInResult { Succeeded = false, IsLockedOut = false };
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                password,
                rememberMe,
                lockoutOnFailure: true);

            return new AccountSignInResult
            {
                Succeeded = result.Succeeded,
                IsLockedOut = result.IsLockedOut,
            };
        }

        public Task SignOutAsync(CancellationToken cancellationToken = default)
        {
            return _signInManager.SignOutAsync();
        }
    }
}
