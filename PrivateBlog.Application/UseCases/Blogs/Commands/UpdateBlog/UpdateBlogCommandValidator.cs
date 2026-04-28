using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

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
                .MaximumLength(128).WithMessage("El título no puede superar los 128 caracteres.");

            RuleFor(x => x.Content)
                .MinimumLength(8).WithMessage("El contenido debe tener al menos 8 caracteres.");

            RuleFor(x => x.SectionId).NotEmpty().WithMessage("Debe seleccionar una sección.");
        }
    }
}
