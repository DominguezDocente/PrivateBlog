using Microsoft.Extensions.DependencyInjection;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IMediator, SimpleMediator>();

            services.AddScoped<IRequestHandler<CreateSectionCommand, Guid>, CreateSectionUseCase>();
            services.AddScoped<IRequestHandler<GetSectionsListQuery, IEnumerable<SectionListItemDTO>>, GetSectionsListUseCase>();

            return services;
        }
    }
}
