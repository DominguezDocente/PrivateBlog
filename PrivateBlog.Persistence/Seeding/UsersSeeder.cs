using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Security;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence.Seeding
{
    public sealed class UsersSeeder
    {
        private const string SeedPassword = "1234";

        private const string SeedAdminEmail = "adminuser@yopmail.com";
        private const string SeedBasicEmail = "basicuser@yopmail.com";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _context;

        public UsersSeeder(UserManager<ApplicationUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await EnsureSeedUserAsync(
                SeedAdminEmail,
                "Seed",
                "Admin",
                PermissionCodesCatalog.Roles.Admin);
            await EnsureSeedUserAsync(
                SeedBasicEmail,
                "Seed",
                "Básico",
                PermissionCodesCatalog.Roles.Basic);
        }

        private async Task SeedRolesAsync()
        {
            await EnsureRoleAsync(PermissionCodesCatalog.Roles.Admin, PermissionCodesCatalog.RoleGrants.Admin);
            await EnsureRoleAsync(PermissionCodesCatalog.Roles.ContentEditor, PermissionCodesCatalog.RoleGrants.ContentEditor);
            await EnsureRoleAsync(PermissionCodesCatalog.Roles.Basic, PermissionCodesCatalog.RoleGrants.Basic);
        }

        private async Task EnsureSeedUserAsync(string email, string firstName, string lastName, string roleName)
        {
            Role role = await _context.Roles.FirstAsync(r => r.Name == roleName);

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    RoleId = role.Id,
                };

                IdentityResult createResult = await _userManager.CreateAsync(user, SeedPassword);
                if (!createResult.Succeeded)
                {
                    string errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"No se pudo crear el usuario semilla '{email}': {errors}");
                }

                return;
            }

            if (user.RoleId != role.Id)
            {
                user.RoleId = role.Id;
                IdentityResult updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    string errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"No se pudo asignar el rol '{roleName}' al usuario '{email}': {errors}");
                }
            }
        }

        private async Task EnsureRoleAsync(string roleName, IReadOnlyList<string> permissionCodes)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role is null)
            {
                role = new Role(roleName);
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }

            List<string> foundCodes = await _context.Permissions
                .Where(p => permissionCodes.Contains(p.Code))
                .Select(p => p.Code)
                .ToListAsync();

            if (foundCodes.Count != permissionCodes.Count)
            {
                string[] missing = permissionCodes.Except(foundCodes).ToArray();
                throw new InvalidOperationException(
                    $"Faltan permisos en BD para el rol '{roleName}'. Ejecute PermissionsSeeder antes de UsersSeeder. Faltan: {string.Join(", ", missing)}");
            }

            List<Guid> permissionIds = await _context.Permissions
                .Where(p => permissionCodes.Contains(p.Code))
                .Select(p => p.Id)
                .ToListAsync();

            List<Guid> existingPermissionIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            List<Guid> toAdd = permissionIds.Except(existingPermissionIds).ToList();

            foreach (Guid permissionId in toAdd)
            {
                await _context.RolePermissions.AddAsync(new RolePermission(role.Id, permissionId));
            }

            if (toAdd.Count > 0)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
