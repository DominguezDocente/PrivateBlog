using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
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

            return builder;
        }
    }
}
