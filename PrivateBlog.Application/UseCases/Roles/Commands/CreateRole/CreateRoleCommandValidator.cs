using FluentValidation;

namespace PrivateBlog.Application.UseCases.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");
        }
    }
}
