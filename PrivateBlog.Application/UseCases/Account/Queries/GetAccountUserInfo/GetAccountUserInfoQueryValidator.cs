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
            RuleFor(q => q.UserId).NotEmpty();
        }
    }
}