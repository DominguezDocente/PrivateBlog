using FluentValidation;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleSections
{
    public class GetAccessibleSectionsQueryValidator : AbstractValidator<GetAccessibleSectionsQuery>
    {
        public GetAccessibleSectionsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es requerido.");
        }
    }
}
