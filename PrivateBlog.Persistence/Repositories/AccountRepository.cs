using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Account;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence.Repositories
{
    public sealed class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> UserHasPermissionAsync(
            string userId,
            string permissionCode,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(permissionCode))
            {
                return false;
            }

            ApplicationUser? appUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (appUser is null)
            {
                return false;
            }

            string code = permissionCode.Trim();

            return await _context.Permissions
                .AsNoTracking()
                .AnyAsync(
                    p => p.Code == code
                         && p.RolePermissions.Any(rp => rp.RoleId == appUser.RoleId),
                    cancellationToken);
        }

        public async Task<UserHeaderInfoDTO?> GetUserHeaderInfoAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            var row = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.UserName,
                    u.Email,
                    RoleName = u.Role.Name,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (row is null)
            {
                return null;
            }

            string fn = row.FirstName?.Trim() ?? string.Empty;
            string ln = row.LastName?.Trim() ?? string.Empty;
            string fullName = $"{fn} {ln}".Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = row.UserName ?? row.Email ?? "Usuario";
            }

            return new UserHeaderInfoDTO
            {
                FirstName = fn,
                LastName = ln,
                FullName = fullName,
                RoleName = row.RoleName,
            };
        }
    }
}
