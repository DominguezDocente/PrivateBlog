using FluentValidation;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSection
{
    public class DeactivateSectionCommandValidator : AbstractValidator<DeactivateSectionCommand>
    {
        public DeactivateSectionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador es obligatorio.");
        }
    }
}
