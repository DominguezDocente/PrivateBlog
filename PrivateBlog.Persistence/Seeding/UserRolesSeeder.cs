using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrivateBlog.Domain.Entities.Users;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence.Seeding
{
    internal sealed class UserRolesSeeder
    {
        private const string AdminRoleName = "Admin";
        private const string EditorRoleName = "Editor";
        private const string ReaderRoleName = "Reader";

        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserRolesSeeder(
            DataContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await EnsureRoleAsync(
                AdminRoleName,
                [
                    "showBlogs", "createBlogs", "updateBlogs", "deleteBlogs",
                    "showSections", "createSections", "updateSections", "deleteSections",
                    "showRoles", "createRoles", "updateRoles", "deleteRoles",
                    "showUsers", "createUsers", "updateUsers", "deleteUsers",
                ]);

            await EnsureRoleAsync(
                EditorRoleName,
                [
                    "showBlogs", "createBlogs", "updateBlogs", "deleteBlogs",
                    "showSections", "createSections", "updateSections", "deleteSections",
                ]);

            await EnsureRoleAsync(
                ReaderRoleName,
                [
                    "showBlogs",
                    "showSections",
                ]);
        }

        private async Task CheckUsersAsync()
        {
            string? adminEmail = _configuration["Seed:AdminEmail"];
            string? adminPassword = _configuration["Seed:AdminPassword"];

            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                await EnsureUserAsync(adminEmail, adminPassword, AdminRoleName);
            }

            string? editorEmail = _configuration["Seed:EditorEmail"];
            string? editorPassword = _configuration["Seed:EditorPassword"];
            if (!string.IsNullOrWhiteSpace(editorEmail) && !string.IsNullOrWhiteSpace(editorPassword))
            {
                await EnsureUserAsync(editorEmail, editorPassword, EditorRoleName);
            }

            string? readerEmail = _configuration["Seed:ReaderEmail"];
            string? readerPassword = _configuration["Seed:ReaderPassword"];
            if (!string.IsNullOrWhiteSpace(readerEmail) && !string.IsNullOrWhiteSpace(readerPassword))
            {
                await EnsureUserAsync(readerEmail, readerPassword, ReaderRoleName);
            }
        }

        private async Task EnsureRoleAsync(string roleName, IReadOnlyCollection<string> permissionCodes)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role is null)
            {
                role = new Role(roleName);
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }

            List<Guid> permissionIds = await _context.Permissions
                .Where(p => permissionCodes.Contains(p.Code))
                .Select(p => p.Id)
                .ToListAsync();

            List<Guid> existingPermissionIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            IEnumerable<Guid> missingPermissionIds = permissionIds.Except(existingPermissionIds);

            foreach (Guid permissionId in missingPermissionIds)
            {
                await _context.RolePermissions.AddAsync(new RolePermission(role.Id, permissionId));
            }

            await _context.SaveChangesAsync();
        }

        private async Task EnsureUserAsync(string email, string password, string roleName)
        {
            string normalizedEmail = email.Trim();
            Guid roleId = await _context.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (roleId == Guid.Empty)
            {
                throw new InvalidOperationException($"No existe el rol '{roleName}' para crear usuario.");
            }

            ApplicationUser? user = await _userManager.FindByEmailAsync(normalizedEmail);
            if (user is null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = normalizedEmail,
                    Email = normalizedEmail,
                    EmailConfirmed = true,
                    RoleId = roleId,
                };

                IdentityResult userCreated = await _userManager.CreateAsync(newUser, password);
                if (!userCreated.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", userCreated.Errors.Select(e => e.Description)));
                }

                return;
            }

            bool requiresUpdate = false;

            if (user.RoleId != roleId)
            {
                user.RoleId = roleId;
                requiresUpdate = true;
            }

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                requiresUpdate = true;
            }

            if (!requiresUpdate)
            {
                return;
            }

            IdentityResult userUpdated = await _userManager.UpdateAsync(user);
            if (!userUpdated.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ", userUpdated.Errors.Select(e => e.Description)));
            }
        }
    }
}
