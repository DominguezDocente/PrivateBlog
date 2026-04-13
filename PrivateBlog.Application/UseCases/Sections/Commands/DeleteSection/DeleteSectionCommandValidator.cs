using FluentValidation;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection
{
    public class DeleteSectionCommandValidator : AbstractValidator<DeleteSectionCommand>
    {
        public DeleteSectionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador es obligatorio.");
        }
    }
}
