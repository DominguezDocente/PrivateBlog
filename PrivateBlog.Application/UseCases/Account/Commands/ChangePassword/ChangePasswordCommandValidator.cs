using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es requerido.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("La contraseña actual es obligatoria.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("La nueva contraseña es obligatoria.")
                .MinimumLength(4).WithMessage("La nueva contraseña debe tener al menos 4 caracteres.");
        }
    }
}
