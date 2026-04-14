using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection
{
    public class UpdateSectionCommandValidator : AbstractValidator<UpdateSectionCommand>
    {
        public UpdateSectionCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("El ID es obligatorio");

            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre es obligatorio")
                                .MinimumLength(4).WithMessage("El nombre debe tener al menos 4 letras.")
                                .MaximumLength(64).WithMessage("El nombre debe tener máximo 64 letras.");
        }
    }
}
