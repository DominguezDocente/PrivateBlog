using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IMediator, SimpleMediator>();

            // Register all IRequestHandler implementations from the assembly containing IMediator
            services.Scan(scan => scan
                    .FromAssembliesOf(typeof(IMediator))
                    .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }
    }
}
