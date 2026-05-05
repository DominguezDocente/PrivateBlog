using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo
{
    public class GetAccountUserInfoQueryValidator : AbstractValidator<GetAccountUserInfoQuery>
    {
        public GetAccountUserInfoQueryValidator()
        {
            RuleFor(u => u.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotNull().WithMessage("UserId cannot be null.");
        }
    }
}
