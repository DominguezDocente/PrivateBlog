using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Controllers;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Data.Seeders;
using PrivateBlog.Web.Helpers.Abstractions;
using PrivateBlog.Web.Helpers.Implementations;
using PrivateBlog.Web.Services.Abstractions;
using PrivateBlog.Web.Services.Implementations;

namespace PrivateBlog.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            // Automapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Services
            AddServices(builder);

            // Toast Notifications
            builder.Services.AddNotyf(config => 
            { 
                config.DurationInSeconds = 10; 
                config.IsDismissable = true; 
                config.Position = NotyfPosition.BottomRight; 
            });

            // IAM
            AddIAM(builder);

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            // Services
            builder.Services.AddScoped<ISectionsService, SectionsService>();
            builder.Services.AddScoped<IBlogsService, BlogsService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddTransient<SeedDb>();

            // Helpers
            builder.Services.AddScoped<ISelectListHelper, SelectListHelper>();
        }

        // IAM: Identity and Access Management
        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequireDigit = false;
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Auth";
                config.ExpireTimeSpan = TimeSpan.FromDays(100);
                config.LoginPath = "/Account/Login";
                config.AccessDeniedPath = "/Error/403";
            });
        }

        public static WebApplication AddCustomWebApplicationConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using IServiceScope scope = scopeFactory.CreateScope();
            SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
            service.SeedAsync().Wait();
        }
    }
}