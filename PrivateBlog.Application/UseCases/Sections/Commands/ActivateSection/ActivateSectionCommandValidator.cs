using FluentValidation;

namespace PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection
{
    public class ActivateSectionCommandValidator : AbstractValidator<ActivateSectionCommand>
    {
        public ActivateSectionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador es obligatorio.");
        }
    }
}
