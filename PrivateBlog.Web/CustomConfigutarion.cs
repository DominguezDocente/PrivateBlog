using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web
{
    public static class CustomConfigutarion
    {

        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(configuration => 
            {
                configuration.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            // Services
            AddServices(builder);

            return builder;
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISectionsService, SectionsService>();
        }
    }
}
