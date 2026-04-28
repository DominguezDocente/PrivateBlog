using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.UpdateBlog
{
    public sealed class UpdateBlogUseCase : IRequestHandler<UpdateBlogCommand>
    {
        private readonly IBlogsRepository _blogsRepository;
        private readonly ISectionsRepository _sectionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBlogUseCase(
            IBlogsRepository blogsRepository,
            ISectionsRepository sectionsRepository,
            IUnitOfWork unitOfWork)
        {
            _blogsRepository = blogsRepository;
            _sectionsRepository = sectionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateBlogCommand command)
        {
            Blog? blog = await _blogsRepository.GetByIdAsync(command.Id);

            if (blog is null)
            {
                throw new BussinesRuleException("El blog no existe.");
            }

            Section? section = await _sectionsRepository.GetByIdAsync(command.SectionId);

            if (section is null || !section.IsActive)
            {
                throw new BussinesRuleException("La sección no existe o no está activa.");
            }

            blog.Update(command.Name, command.Content, command.SectionId, command.IsPublished);

            try
            {
                await _blogsRepository.UpdateAsync(blog);
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