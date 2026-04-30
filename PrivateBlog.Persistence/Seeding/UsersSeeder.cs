using Microsoft.AspNetCore.Identity;
using PrivateBlog.Persistence.Entitities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Seeding
{
    public class UsersSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await CheckUsersAsync();
        }

        private async Task CheckUsersAsync()
        {
            string email = "adminuser@yopmail.com";

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "Seed Admin",
                    LastName = ".",
                };

                await _userManager.CreateAsync(user, "1234");
            }
        }
    }
}
