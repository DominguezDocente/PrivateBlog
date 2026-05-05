using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using Microsoft.AspNetCore.Identity;
using PrivateBlog.Persistence.Entities;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo;
using Microsoft.EntityFrameworkCore;


namespace PrivateBlog.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _context;

        public AccountRepository(SignInManager<ApplicationUser> signinManager, UserManager<ApplicationUser> userManager, DataContext context)
        {
            _signinManager = signinManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<UserAccountInfoDTO> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            ApplicationUser? user = await _context.Users.Include(u => u.Role)
                                                        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return null;
            }

            return new UserAccountInfoDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleName = user.Role.Name
            };
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

        public async Task<bool> UserHasPermissionAsync(string userId, string permissionCode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(permissionCode))
            {
                return false;
            }

            ApplicationUser? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return false;
            }

            return await _context.Permissions.AnyAsync(p => p.Code == permissionCode
                                                           && p.RolePermissions.Any(rp => rp.RoleId == user.RoleId));
        }
    }
}
