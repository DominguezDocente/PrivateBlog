using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Data.Seeders
{
    public class AccountSeeder
    {
        private readonly IUsersService _usersService;
        private readonly IAccountService _accountService;
        private readonly DataContext _context;

        private const string CONTENT_MANAGER_ROLE_NAME = "Gestor de contenido";
        private const string BASIC_ROLE_NAME = "Basic";

        public AccountSeeder(IUsersService usersService, DataContext context, IAccountService accountService)
        {
            _usersService = usersService;
            _context = context;
            _accountService = accountService;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await AdminRoleAsync();
            await ContentManagerRoleAsync();
            await BasicRoleAsync();
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name ==  Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole
                {
                    Id = Guid.NewGuid(),
                    Name = Env.SUPER_ADMIN_ROLE_NAME
                };

                await _context.PrivateBlogRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task BasicRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == BASIC_ROLE_NAME);

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole
                {
                    Id = Guid.NewGuid(),
                    Name = BASIC_ROLE_NAME
                };

                await _context.PrivateBlogRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ContentManagerRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == CONTENT_MANAGER_ROLE_NAME);

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole
                {
                    Id = Guid.NewGuid(),
                    Name = CONTENT_MANAGER_ROLE_NAME
                };

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

        private async Task CheckUsersAsync()
        {
            await AdminUserAsync();
            await ContentManagerUserAsync();
            await BasicUserAsync();
        }

        private async Task AdminUserAsync()
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "admin@privateblog.com");

            if (user is null)
            {
                PrivateBlogRole adminRole = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "admin@privateblog.com",
                    UserName = "admin@privateblog.com",
                    FirstName = "Administrador",
                    LastName = "Admin",
                    PhoneNumber = "3000000001",
                    Document = "1111",
                    PrivateBlogRoleId = adminRole.Id,
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _accountService.GenerateConfirmationTokenAsync(user)).Result;

                await _accountService.ConfirmUserAsync(user, token);
            }
        }

        private async Task ContentManagerUserAsync()
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "jhond@privateblog.com");

            if (user is null)
            {
                PrivateBlogRole role = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == CONTENT_MANAGER_ROLE_NAME);

                user = new User
                {
                    Email = "jhond@privateblog.com",
                    UserName = "jhond@privateblog.com",
                    FirstName = "Jhon",
                    LastName = "Doe",
                    PhoneNumber = "3000000002",
                    Document = "2222",
                    PrivateBlogRoleId = role.Id,
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _accountService.GenerateConfirmationTokenAsync(user)).Result;

                await _accountService.ConfirmUserAsync(user, token);
            }
        }

        private async Task BasicUserAsync()
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "anad@privateblog.com");

            if (user is null)
            {
                PrivateBlogRole role = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == BASIC_ROLE_NAME);

                user = new User
                {
                    Email = "anad@privateblog.com",
                    UserName = "anad@privateblog.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "3000000003",
                    Document = "3333",
                    PrivateBlogRoleId = role.Id,
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _accountService.GenerateConfirmationTokenAsync(user)).Result;

                await _accountService.ConfirmUserAsync(user, token);
            }
        }
    }
}
