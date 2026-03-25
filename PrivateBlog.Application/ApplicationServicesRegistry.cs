using Microsoft.Extensions.DependencyInjection;
using PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection;
using PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById;
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
            services.AddScoped<IRequestHandler<GetSectionByIdQuery, SectionDetailDTO?>, GetSectionByIdUseCase>();
            services.AddScoped<IRequestHandler<UpdateSectionCommand>, UpdateSectionUseCase>();
            services.AddScoped<IRequestHandler<DeleteSectionCommand>, DeleteSectionUseCase>();
            services.AddScoped<IRequestHandler<ActivateSectionCommand>, ActivateSectionUseCase>();
            services.AddScoped<IRequestHandler<DeactivateSectionCommand>, DeactivateSectionUseCase>();

            return services;
        }
    }
}
