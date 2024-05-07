using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core.Middlewares;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Data.Seeders;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web
{
    public static class CustomConfiguration
    {
        #region Builder

        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(conf =>
            {
                conf.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            builder.Services.AddHttpContextAccessor();

            // Services
            AddServices(builder);

            // Identity and Access Managnet
            AddIAM(builder);

            // Toast
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            return builder;
        }

        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(x =>
            {
                x.User.RequireUniqueEmail = true;
                x.Password.RequireDigit = false;
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequiredLength = 4;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Auth";
                options.LoginPath = "/Account/Login"; // Ruta de inicio de sesión
                options.AccessDeniedPath = "/Account/NotAuthorized"; // Ruta de acceso denegado
            });

            builder.Services.AddAuthorization();
        }

        private static void AddServices(this WebApplicationBuilder builder)
        {
            // Services
            builder.Services.AddScoped<IBlogsService, BlogsService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<ISectionsService, SectionsService>();
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            // Helpers
            builder.Services.AddScoped<ICombosHelper, CombosHelper>(); 
            builder.Services.AddScoped<IConverterHelper, ConverterHelper>(); 
        }

        #endregion Builder

        #region App

        public static WebApplication AddCustomConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopedFactory!.CreateScope())
            {
                SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
                service!.SeedAsync().Wait();
            }
        }

        #endregion App
    }
}