using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            // Registrar AutoMapper con la búsqueda automática de Profile
            builder.Services.AddAutoMapper(typeof(Program));

            // Services
            AddServices(builder);

            return builder;
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            // Services
            builder.Services.AddScoped<ISectionsService, SectionsService>();
        }
    }
}
