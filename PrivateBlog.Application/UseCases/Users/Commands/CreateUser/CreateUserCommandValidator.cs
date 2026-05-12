using FluentValidation;

namespace PrivateBlog.Application.UseCases.Users.Commands.CreateUser
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("El nombre es obligatorio.")
                                     .MaximumLength(64).WithMessage("El nombre no puede exceder 64 caracteres.");

            RuleFor(c => c.LastName).NotEmpty().WithMessage("El apellido es obligatorio.")
                                    .MaximumLength(64).WithMessage("El apellido no puede exceder 64 caracteres.");

            RuleFor(c => c.Email).NotEmpty().WithMessage("El correo es obligatorio.")
                                 .EmailAddress().WithMessage("El correo no es válido.");

            RuleFor(c => c.Password).NotEmpty().WithMessage("La contraseña es obligatoria.")
                                    .MinimumLength(4).WithMessage("La contraseña debe tener al menos 4 caracteres.");

            RuleFor(c => c.RoleId).NotEmpty().WithMessage("El rol es obligatorio.");
        }
    }
}
