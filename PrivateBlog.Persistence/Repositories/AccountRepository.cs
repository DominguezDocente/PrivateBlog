using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogById;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogsBySection;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleSections;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using Microsoft.AspNetCore.Identity;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Domain.Entities.Sections;
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

        public async Task<AccountSignInResult> SignInAsync(
            string userName,
            string password,
            bool rememberMe,
            bool useCookieAuth = true,
            CancellationToken cancellationToken = default)
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

            SignInResult result = useCookieAuth
                ? await _signinManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: true)
                : await _signinManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

            return new AccountSignInResult
            {
                Succeeded = result.Succeeded,
                IsLockedOut = result.IsLockedOut,
                UserId = result.Succeeded ? user.Id : null
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

        public async Task<IReadOnlyList<AccessibleSectionItemDTO>> GetAccessibleSectionsAsync(string userId, CancellationToken cancellationToken = default)
        {
            Guid roleId = await GetRoleIdAsync(userId, cancellationToken);

            return await _context.Sections
                .AsNoTracking()
                .Where(s => s.IsActive && s.RoleSections.Any(rs => rs.RoleId == roleId))
                .OrderBy(s => s.Name)
                .Select(s => new AccessibleSectionItemDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    PublishedBlogsCount = s.Blogs.Count(b => b.IsPublished),
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<AccessibleSectionBlogsDTO> GetAccessibleBlogsBySectionAsync(string userId, Guid sectionId, CancellationToken cancellationToken = default)
        {
            Guid roleId = await GetRoleIdAsync(userId, cancellationToken);

            Section? section = await _context.Sections
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    s => s.Id == sectionId
                         && s.IsActive
                         && s.RoleSections.Any(rs => rs.RoleId == roleId),
                    cancellationToken);

            if (section is null)
            {
                throw new BussinesRuleException("No tiene acceso a esta sección.");
            }

            List<AccessibleBlogListItemDTO> blogs = await _context.Blogs
                .AsNoTracking()
                .Where(b => b.SectionId == sectionId && b.IsPublished)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new AccessibleBlogListItemDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    CreatedAt = b.CreatedAt,
                })
                .ToListAsync(cancellationToken);

            return new AccessibleSectionBlogsDTO
            {
                SectionId = section.Id,
                SectionName = section.Name,
                Blogs = blogs,
            };
        }

        public async Task<AccessibleBlogDetailDTO> GetAccessibleBlogByIdAsync(string userId, Guid blogId, CancellationToken cancellationToken = default)
        {
            Guid roleId = await GetRoleIdAsync(userId, cancellationToken);

            Blog? blog = await _context.Blogs
                .AsNoTracking()
                .Include(b => b.Section)
                .FirstOrDefaultAsync(
                    b => b.Id == blogId
                         && b.IsPublished
                         && b.Section.IsActive
                         && b.Section.RoleSections.Any(rs => rs.RoleId == roleId),
                    cancellationToken);

            if (blog is null)
            {
                throw new BussinesRuleException("No tiene acceso a este blog.");
            }

            return new AccessibleBlogDetailDTO
            {
                Id = blog.Id,
                Name = blog.Name,
                Content = blog.Content,
                SectionId = blog.SectionId,
                SectionName = blog.Section.Name,
                CreatedAt = blog.CreatedAt,
            };
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

        private async Task<Guid> GetRoleIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BussinesRuleException("El usuario es requerido.");
            }

            Guid roleId = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.RoleId)
                .FirstOrDefaultAsync(cancellationToken);

            if (roleId == Guid.Empty)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            return roleId;
        }
    }
}
