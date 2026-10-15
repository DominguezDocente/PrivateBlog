using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
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

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            // Services
            builder.Services.AddScoped<ISectionsService, SectionsService>();
            builder.Services.AddScoped<IBlogsService, BlogsService>();

            // Helpers
            builder.Services.AddScoped<ISelectListHelper, SelectListHelper>();
        }


        public static WebApplication AddCustomWebApplicationConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            return app;
        }
    }
}