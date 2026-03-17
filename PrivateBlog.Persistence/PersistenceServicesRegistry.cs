using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Persistence.Respositories;
using PrivateBlog.Persistence.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer("name=PrivateBlogConnectionString");
            });

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<ISectionsRepository, SectionsRepository>();

            return services;
        }
    }
}
