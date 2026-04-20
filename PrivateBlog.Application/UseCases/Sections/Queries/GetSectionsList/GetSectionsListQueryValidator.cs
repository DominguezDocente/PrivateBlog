using FluentValidation;
using PrivateBlog.Application.Contracts.Pagination;

namespace PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList
{
    public class GetSectionsListQueryValidator : AbstractValidator<GetSectionsListQuery>
    {
        public GetSectionsListQueryValidator()
        {
            RuleFor(x => x.Pagination.PageNumber).GreaterThan(0);
            RuleFor(x => x.Pagination.PageSize)
                .InclusiveBetween(1, PaginationRequest.MaxPageSize);

            When(x => !string.IsNullOrWhiteSpace(x.NameFilter), () =>
            {
                RuleFor(x => x.NameFilter!)
                    .MaximumLength(64);
            });
        }
    }
}
