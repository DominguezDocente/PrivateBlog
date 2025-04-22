using Microsoft.EntityFrameworkCore;
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
                PrivateBlogRole adminRole = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "manuel@yopmail.com",
                    FirstName = "Manuel",
                    LastName = "Domínguez",
                    PhoneNumber = "3000000000",
                    UserName = "manuel@yopmail.com",
                    Document = "111111",
                    PrivateBlogRole = adminRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // ContentNamager
            user = await _usersService.GetUserAsync("anad@yopmail.com");

            if (user is null)
            {
                PrivateBlogRole contentManagerRole = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Name == "Gestor de contenido");

                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "310000000",
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
            await ContentManagerRoleAsync();
        }

        private async Task ContentManagerRoleAsync()
        {
            bool exists = await _context.PrivateBlogRoles.AnyAsync(r => r.Name == "Gestor de contenido");

            if (!exists)
            {
                PrivateBlogRole role = new PrivateBlogRole { Name = "Gestor de contenido" };
                await _context.PrivateBlogRoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Secciones" || p.Module == "Blogs")
                                                                         .ToListAsync();

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
