using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Services;
using System.Runtime.CompilerServices;

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

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Services
            AddServices(builder);

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISectionsService, SectionsService>();
        }
    }
}
