using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;

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

            return builder;
        }
    }
}
