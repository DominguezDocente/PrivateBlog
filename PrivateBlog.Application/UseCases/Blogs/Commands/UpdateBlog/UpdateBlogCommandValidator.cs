using FluentValidation;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.UpdateBlog
{
    public sealed class UpdateBlogCommandValidator : AbstractValidator<UpdateBlogCommand>
    {
        public UpdateBlogCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MinimumLength(3).WithMessage("El título debe tener al menos 3 caracteres.")
                .MaximumLength(200).WithMessage("El título no puede superar los 200 caracteres.");

            RuleFor(x => x.Content)
                .MaximumLength(500_000).WithMessage("El contenido es demasiado largo.");

            RuleFor(x => x.SectionId).NotEmpty().WithMessage("Debe seleccionar una sección.");
        }
    }
}
