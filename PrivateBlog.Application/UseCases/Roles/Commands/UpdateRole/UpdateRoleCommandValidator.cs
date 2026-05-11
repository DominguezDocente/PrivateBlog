using FluentValidation;

namespace PrivateBlog.Application.UseCases.Roles.Commands.UpdateRole
{
    public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("El ID es obligatorio.");

            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");
        }
    }
}
