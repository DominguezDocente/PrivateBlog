using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Data.Seeders
{
    public class AccountSeeder
    {
        private readonly IUsersService _usersService;
        private readonly DataContext _context;

        public AccountSeeder(IUsersService usersService, DataContext context)
        {
            _usersService = usersService;
            _context = context;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await AdminRoleAsync();
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

        private async Task CheckUsersAsync()
        {
            // Admin
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

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;

                await _usersService.ConfirmUserAsync(user, token);
            }
        }
    }
}
