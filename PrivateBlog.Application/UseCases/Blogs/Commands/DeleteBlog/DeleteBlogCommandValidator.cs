using FluentValidation;

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
