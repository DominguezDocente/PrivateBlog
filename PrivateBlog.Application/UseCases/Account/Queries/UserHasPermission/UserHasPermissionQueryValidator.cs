using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.UserHasPermission
{
    public sealed class UserHasPermissionQueryValidator : AbstractValidator<UserHasPermissionQuery>
    {
        public UserHasPermissionQueryValidator()
        {
            RuleFor(q => q.UserId).NotEmpty();
            RuleFor(q => q.PermissionCode).NotEmpty();
        }
    }
}
