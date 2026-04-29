using FluentValidation;

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
