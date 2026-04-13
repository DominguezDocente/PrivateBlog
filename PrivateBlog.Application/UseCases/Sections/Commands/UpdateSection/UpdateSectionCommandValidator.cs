using FluentValidation;

namespace PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection
{
    public class UpdateSectionCommandValidator : AbstractValidator<UpdateSectionCommand>
    {
        public UpdateSectionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador es obligatorio.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(4).WithMessage("El nombre debe tener al menos 4 caracteres.")
                .MaximumLength(64).WithMessage("El nombre no puede superar los 64 caracteres.");
        }
    }
}
