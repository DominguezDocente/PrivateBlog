using FluentValidation;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogsBySection
{
    public class GetAccessibleBlogsBySectionQueryValidator : AbstractValidator<GetAccessibleBlogsBySectionQuery>
    {
        public GetAccessibleBlogsBySectionQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es requerido.");

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage("La sección es requerida.");
        }
    }
}
