using PrivateBlog.Application.Contracts.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.DeleteBlog
{
    public sealed class DeleteBlogUseCase : IRequestHandler<DeleteBlogCommand>
    {
        private readonly IBlogsRepository _blogsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBlogUseCase(IBlogsRepository blogsRepository, IUnitOfWork unitOfWork)
        {
            _blogsRepository = blogsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteBlogCommand command)
        {
            Blog? blog = await _blogsRepository.GetByIdAsync(command.Id);

            if (blog is null)
            {
                throw new BusinessRuleException("El blog no existe.");
            }

            try
            {
                await _blogsRepository.DeleteAsync(blog);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
