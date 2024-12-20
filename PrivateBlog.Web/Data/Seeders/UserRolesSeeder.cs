﻿using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public UserRolesSeeder(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await CheckRoles();
            await CheckUsers();
        }

        private async Task CheckUsers()
        {
            // Admin
            User? user = await _usersService.GetUserAsync("manuel@yopmail.com");

            if (user is null)
            {
                PrivateBlogRole adminRole = _context.PrivateBlogRoles.FirstOrDefault(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "manuel@yopmail.com",
                    FirstName = "Manuel",
                    LastName = "Dominguez",
                    PhoneNumber = "30000000",
                    UserName = "manuel@yopmail.com",
                    Document = "11111",
                    PrivateBlogRole = adminRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Content Manager
            user = await _usersService.GetUserAsync("anad@yopmail.com");

            if (user is null)
            {
                PrivateBlogRole contentManagerRole = _context.PrivateBlogRoles.FirstOrDefault(r => r.Name == "Gestor de contenido");

                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "31111111",
                    UserName = "anad@yopmail.com",
                    Document = "22222",
                    PrivateBlogRole = contentManagerRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRoles()
        {
            await AdminRoleAsync();
            await ContentManagerAsync();
            await UserManagerAsync();
        }

        private async Task UserManagerAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == "Gestor de usuarios");

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole { Name = "Gestor de usuarios" };
                await _context.PrivateBlogRoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Usuarios").ToListAsync();

                foreach (Permission permission in permissions) 
                {
                    await _context.RolePermissions.AddAsync(new RolePermission { Permission = permission, Role = role });
                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task ContentManagerAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == "Gestor de contenido");

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole { Name = "Gestor de contenido" };
                await _context.PrivateBlogRoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Secciones" || p.Module == "Blogs").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission { Permission = permission, Role = role });
                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists) 
            {
                PrivateBlogRole role = new PrivateBlogRole { Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.PrivateBlogRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
