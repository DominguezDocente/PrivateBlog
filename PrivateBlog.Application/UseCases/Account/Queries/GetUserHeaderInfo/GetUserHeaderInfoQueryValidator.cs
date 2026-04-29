using FluentValidation;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetUserHeaderInfo
{
    public sealed class GetUserHeaderInfoQueryValidator : AbstractValidator<GetUserHeaderInfoQuery>
    {
        public GetUserHeaderInfoQueryValidator()
        {
            RuleFor(q => q.UserId).NotEmpty();
        }
    }
}
