using FluentValidation;
using PrivateBlog.Application.Contracts.Pagination;

namespace PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList
{
    public sealed class GetBlogsListQueryValidator : AbstractValidator<GetBlogsListQuery>
    {
        public GetBlogsListQueryValidator()
        {
            RuleFor(x => x.Pagination.PageNumber).GreaterThan(0);
            RuleFor(x => x.Pagination.PageSize)
                .InclusiveBetween(1, PaginationRequest.MaxPageSize);

            When(x => !string.IsNullOrWhiteSpace(x.NameFilter), () =>
            {
                RuleFor(x => x.NameFilter!)
                    .MaximumLength(200);
            });
        }
    }
}
