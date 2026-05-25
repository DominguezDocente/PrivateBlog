using FluentValidation;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile
{
    public class GetAccountProfileQueryValidator : AbstractValidator<GetAccountProfileQuery>
    {
        public GetAccountProfileQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es requerido.");
        }
    }
}
