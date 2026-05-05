using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.UserHasPermission
{
    public class UserHasPermissionQueryValidator : AbstractValidator<UserHasPermissionQuery>
    {
        public UserHasPermissionQueryValidator()
        {

            RuleFor(u => u.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotNull().WithMessage("UserId cannot be null.");

            RuleFor(u => u.PermissionCode)
                .NotEmpty().WithMessage("PermissionCode is required.")
                .NotNull().WithMessage("PermissionCode cannot be null.");
        }
    }
}
