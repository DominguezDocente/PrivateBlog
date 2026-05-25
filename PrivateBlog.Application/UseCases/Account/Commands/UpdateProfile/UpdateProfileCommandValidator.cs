using FluentValidation;

namespace PrivateBlog.Application.UseCases.Account.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es requerido.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(64).WithMessage("El nombre no puede exceder 64 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(64).WithMessage("El apellido no puede exceder 64 caracteres.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(32).WithMessage("El teléfono no puede exceder 32 caracteres.");
        }
    }
}
