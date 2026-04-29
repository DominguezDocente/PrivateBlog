using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PrivateBlog.Application.Contracts.Account;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Authentication;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using PrivateBlog.Application.UseCases.Account.Commands.Logout;
using PrivateBlog.Application.UseCases.Account.Queries.GetUserHeaderInfo;
using PrivateBlog.Application.UseCases.Account.Queries.UserHasPermission;
using PrivateBlog.Application.UseCases.Blogs.Commands.CreateBlog;
using PrivateBlog.Application.UseCases.Blogs.Commands.DeleteBlog;
using PrivateBlog.Application.UseCases.Blogs.Commands.UpdateBlog;
using PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogById;
using PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList;
using PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection;
using PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionOptions;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateSectionCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateSectionCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateBlogCommandValidator>();

            services.AddTransient<IMediator, SimpleMediator>();

            services.AddScoped<IRequestHandler<GetSectionsListQuery, PaginationResponse<SectionListItemDTO>>, GetSectionsListUseCase>();
            services.AddScoped<IRequestHandler<GetSectionByIdQuery, SectionDetailDTO?>, GetSectionByIdUseCase>();
            services.AddScoped<IRequestHandler<GetSectionOptionsQuery, IReadOnlyList<SectionOptionDTO>>, GetSectionOptionsUseCase>();
            services.AddScoped<IRequestHandler<GetBlogsListQuery, PaginationResponse<BlogListItemDTO>>, GetBlogsListUseCase>();
            services.AddScoped<IRequestHandler<GetBlogByIdQuery, BlogDetailDTO?>, GetBlogByIdUseCase>();
            services.AddScoped<IRequestHandler<CreateSectionCommand, Guid>, CreateSectionUseCase>();
            services.AddScoped<IRequestHandler<UpdateSectionCommand>, UpdateSectionUseCase>();
            services.AddScoped<IRequestHandler<DeleteSectionCommand>, DeleteSectionUseCase>();
            services.AddScoped<IRequestHandler<ActivateSectionCommand>, ActivateSectionUseCase>();
            services.AddScoped<IRequestHandler<DeactivateSectionCommand>, DeactivateSectionUseCase>();
            services.AddScoped<IRequestHandler<CreateBlogCommand, Guid>, CreateBlogUseCase>();
            services.AddScoped<IRequestHandler<UpdateBlogCommand>, UpdateBlogUseCase>();
            services.AddScoped<IRequestHandler<DeleteBlogCommand>, DeleteBlogUseCase>();
            services.AddScoped<IRequestHandler<LoginCommand, AccountSignInResult>, LoginUseCase>();
            services.AddScoped<IRequestHandler<LogoutCommand>, LogoutUseCase>();
            services.AddScoped<IRequestHandler<UserHasPermissionQuery, bool>, UserHasPermissionUseCase>();
            services.AddScoped<IRequestHandler<GetUserHeaderInfoQuery, UserHeaderInfoDTO?>, GetUserHeaderInfoUseCase>();

            return services;
        }
    }
}
