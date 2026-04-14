using FluentValidation;

namespace PrivateBlog.Application.UseCases.Sections.Commands.CreateSection
{
    public class CreateSectionCommandValidator : AbstractValidator<CreateSectionCommand>
    {
        public CreateSectionCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre es obligatorio")
                                .MinimumLength(4).WithMessage("El nombre debe tener al menos 4 letras.")
                                .MaximumLength(64).WithMessage("El nombre debe tener máximo 64 letras.");
        }
    }
}
