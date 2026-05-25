using FluentValidation;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogById
{
    public class GetAccessibleBlogByIdQueryValidator : AbstractValidator<GetAccessibleBlogByIdQuery>
    {
        public GetAccessibleBlogByIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es requerido.");

            RuleFor(x => x.BlogId)
                .NotEmpty().WithMessage("El blog es requerido.");
        }
    }
}
