using Microsoft.AspNetCore.Identity;
using PrivateBlog.Application.Contracts.Security;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PrivateBlog.Persistence.Seeding
{
    public class UsersSeeder
    {
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
            await SeedUsersAsync();
        }

        private async Task SeedUsersAsync()
        {
            await CheckUsersAsync("adminuser@yopmail.com", "Seed Admin", "", RolesCatalog.ADMIN);
            await CheckUsersAsync("basicuser@yopmail.com", "Jhon", "Doe", RolesCatalog.USER);
        }

        private async Task SeedRolesAsync()
        {
            await CheckRolesAsync(RolesCatalog.ADMIN, PermissionCodesCatalog.All.Select(s => s.Code).ToList());

            await CheckRolesAsync(RolesCatalog.CONTENT_EDITOR, new List<string>
            {
                PermissionCodesCatalog.SHOW_BLOGS,
                PermissionCodesCatalog.CREATE_BLOGS, 
                PermissionCodesCatalog.EDIT_BLOGS,
                PermissionCodesCatalog.DELETE_BLOGS,

                PermissionCodesCatalog.SHOW_SECTIONS,
                PermissionCodesCatalog.CREATE_SECTIONS,
            });

            await CheckRolesAsync(RolesCatalog.USER, new List<string>
            {
                PermissionCodesCatalog.SHOW_BLOGS,

                PermissionCodesCatalog.SHOW_SECTIONS,
            });

        }

        private async Task CheckUsersAsync(string email, string firstName, string lastName, string roleName)
        {
            Role role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

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
                    RoleId = role.Id
                };

                await _userManager.CreateAsync(user, "1234");
            }
        }

        private async Task CheckRolesAsync(string roleName, List<string> permissionCodes)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role is null)
            {
                role = new Role(roleName);
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }

            List<Guid> permissionIds = await _context.Permissions.Where(p => permissionCodes.Contains(p.Code))
                                                                 .Select(p => p.Id)
                                                                 .ToListAsync();

            List<Guid> existingPermissionIds = await _context.RolePermissions
                                                             .Where(rp => rp.RoleId == role.Id)
                                                             .Select(rp => rp.PermissionId)
                                                             .ToListAsync();

            List<Guid> toAdd = permissionIds.Except(existingPermissionIds)
                                            .ToList();

            foreach (Guid permissionId in toAdd)
            {
                RolePermission rolePermission = new RolePermission(role.Id, permissionId);
                await _context.RolePermissions.AddAsync(rolePermission);
            }

            await _context.SaveChangesAsync();
        }
    }
}
