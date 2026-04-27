using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrivateBlog.Application.Contracts.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Persistence.Repositories;
using PrivateBlog.Persistence.Seeding;
using PrivateBlog.Persistence.UnitOfWorks;

namespace PrivateBlog.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer("name=PrivateBlogConnectionString"));

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<ISectionsRepository, SectionsRepository>();
            services.AddScoped<IBlogsRepository, BlogsRepository>();

            services.AddTransient<SeedDb>();

            return services;
        }
    }
}
