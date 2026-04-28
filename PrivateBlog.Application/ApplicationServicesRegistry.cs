using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Blogs.Commands.CreateBlog;
using PrivateBlog.Application.UseCases.Blogs.Commands.DeleteBlog;
using PrivateBlog.Application.UseCases.Blogs.Commands.UpdateBlog;
using PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogById;
using PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList;
using PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSeccion;
using PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection;
using PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Mediator
            services.AddTransient<IMediator, SimpleMediator>();

            // Use cases
            services.AddScoped<IRequestHandler<GetSectionsListQuery, PaginationResponse<SectionListItemDTO>>, GetSectionsListUseCase>();
            services.AddScoped<IRequestHandler<GetSectionByIdQuery, SectionDetailDTO>, GetSectionByIdUseCase>();
            services.AddScoped<IRequestHandler<CreateSectionCommand, Guid>, CreateSectionUseCase>();
            services.AddScoped<IRequestHandler<UpdateSectionCommand>, UpdateSectionUseCase>();
            services.AddScoped<IRequestHandler<DeleteSectionCommand>, DeleteSectionUseCase>();
            services.AddScoped<IRequestHandler<ActivateSectionCommand>, ActivateSectionUseCase>();
            services.AddScoped<IRequestHandler<DeactivateSeccionCommand>, DeactivateSeccionUseCase>();


            services.AddScoped<IRequestHandler<GetSectionOptionsQuery, IReadOnlyList<SectionOptionDTO>>, GetSectionOptionsUseCase>();
            services.AddScoped<IRequestHandler<GetBlogsListQuery, PaginationResponse<BlogListItemDTO>>, GetBlogsListUseCase>();
            services.AddScoped<IRequestHandler<GetBlogByIdQuery, BlogDetailDTO?>, GetBlogByIdUseCase>();
            services.AddScoped<IRequestHandler<CreateBlogCommand, Guid>, CreateBlogUseCase>();
            services.AddScoped<IRequestHandler<UpdateBlogCommand>, UpdateBlogUseCase>();
            services.AddScoped<IRequestHandler<DeleteBlogCommand>, DeleteBlogUseCase>();

            // Validators
            services.AddValidatorsFromAssemblyContaining<CreateSectionCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateSectionCommandValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateBlogCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateBlogCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<DeleteBlogCommandValidator>(); 

            return services;
        }
    }
}
