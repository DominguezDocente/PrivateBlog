using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence.Seeding
{
    /// <summary>
    /// Crea el usuario admin si existen Seed:AdminEmail y Seed:AdminPassword (sin roles).
    /// </summary>
    internal sealed class UsersSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersSeeder(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            string? adminEmail = _configuration["Seed:AdminEmail"];
            string? adminPassword = _configuration["Seed:AdminPassword"];

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                return;
            }

            string emailNormalized = adminEmail.Trim();
            ApplicationUser? existing = await _userManager.FindByEmailAsync(emailNormalized);
            if (existing is not null)
            {
                return;
            }

            var admin = new ApplicationUser
            {
                UserName = emailNormalized,
                Email = emailNormalized,
                EmailConfirmed = true,
            };

            IdentityResult userCreated = await _userManager.CreateAsync(admin, adminPassword);
            if (!userCreated.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ", userCreated.Errors.Select(e => e.Description)));
            }
        }
    }
}
