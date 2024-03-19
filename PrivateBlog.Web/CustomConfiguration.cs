using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
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

            // Services
            AddServices(builder);

            // Toast
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            return builder;
        }

        private static void AddServices(this WebApplicationBuilder builder)
        {
            // Services
            builder.Services.AddScoped<ISectionsService, SectionsService>();

            // Helpers

        }
        #endregion

        #region App

        public static WebApplication AddCustomConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            return app;
        }

        #endregion
    }
}
