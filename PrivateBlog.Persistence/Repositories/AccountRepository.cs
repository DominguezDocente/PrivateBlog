using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using Microsoft.AspNetCore.Identity;
using PrivateBlog.Persistence.Entities;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Domain.Exceptions;
using System.Linq;


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

        public async Task<AccountProfileDTO> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BussinesRuleException("El usuario es requerido.");
            }

            ApplicationUser? user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            return new AccountProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
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

        public async Task UpdateProfileAsync(string userId, string firstName, string lastName, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            user.FirstName = firstName.Trim();
            user.LastName = lastName.Trim();
            user.PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new BussinesRuleException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                throw new BussinesRuleException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
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
