using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                                .MaximumLength(64).WithMessage("El nombre no puede exceder 64 caracteres.")
                                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");
        }
    }
}
