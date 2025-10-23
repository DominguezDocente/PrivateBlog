using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Services.Abtractions;

namespace PrivateBlog.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;
        private const string CONTENT_MANAGER_ROLE_NAME = "Gestor de contenido";
        private const string BASIC_ROLE_NAME = "Basic";

        public UserRolesSeeder(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await AdminRoleAsync();
            await BasicRoleAsync();
            await ContentManagerRoleAsync();
        }

        private async Task CheckUsersAsync()
        {
            // Admin
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "manuel@yopmail.com");

            if (user is null)
            {
                PrivateBlogRole adminRole = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User 
                {
                    Email = "manuel@yopmail.com",
                    FirstName = "Manuel",
                    LastName = "Domínguez",
                    PhoneNumber = "3000000000",
                    UserName = "manuel@yopmail.com",
                    Document = "1111",
                    PrivateBlogRoleId = adminRole!.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }

            // Content manager
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "anad@yopmail.com");

            if (user is null)
            {
                PrivateBlogRole contentManagerRole = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == CONTENT_MANAGER_ROLE_NAME);

                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "3100000000",
                    UserName = "anad@yopmail.com",
                    Document = "222",
                    PrivateBlogRoleId = contentManagerRole!.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }

            // Basic
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "jhond@yopmail.com");

            if (user is null)
            {
                PrivateBlogRole basicRole = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == BASIC_ROLE_NAME);

                user = new User
                {
                    Email = "jhond@yopmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe",
                    PhoneNumber = "3200000000",
                    UserName = "jhond@yopmail.com",
                    Document = "333",
                    PrivateBlogRoleId = basicRole!.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole { Id = Guid.NewGuid(), Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.PrivateBlogRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task BasicRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == BASIC_ROLE_NAME);

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole { Id = Guid.NewGuid(), Name = BASIC_ROLE_NAME };
                await _context.PrivateBlogRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ContentManagerRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == CONTENT_MANAGER_ROLE_NAME);

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole { Id = Guid.NewGuid(), Name = CONTENT_MANAGER_ROLE_NAME };
                await _context.PrivateBlogRoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Secciones" || p.Module == "Blogs")
                                                                         .ToListAsync();
                foreach(Permission permission in permissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission { PermissionId = permission.Id, PrivateBlogRoleId = role.Id });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
