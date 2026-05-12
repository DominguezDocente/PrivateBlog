using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("El ID es obligatorio.");

            RuleFor(c => c.FirstName).NotEmpty().WithMessage("El nombre es obligatorio.")
                                     .MaximumLength(64).WithMessage("El nombre no puede exceder 64 caracteres.");

            RuleFor(c => c.LastName).NotEmpty().WithMessage("El apellido es obligatorio.")
                                    .MaximumLength(64).WithMessage("El apellido no puede exceder 64 caracteres.");

            RuleFor(c => c.Email).NotEmpty().WithMessage("El correo es obligatorio.")
                                 .EmailAddress().WithMessage("El correo no es válido.");

            RuleFor(c => c.RoleId).NotEmpty().WithMessage("El rol es obligatorio.");
        }
    }
}
