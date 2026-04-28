using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.DeleteBlog
{
    public sealed class DeleteBlogCommandValidator : AbstractValidator<DeleteBlogCommand>
    {
        public DeleteBlogCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
