using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Blogs.Commands.CreateBlog
{
    public sealed class CreateBlogUseCase : IRequestHandler<CreateBlogCommand, Guid>
    {
        private readonly IBlogsRepository _blogsRepository;
        private readonly ISectionsRepository _sectionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBlogUseCase(
            IBlogsRepository blogsRepository,
            ISectionsRepository sectionsRepository,
            IUnitOfWork unitOfWork)
        {
            _blogsRepository = blogsRepository;
            _sectionsRepository = sectionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateBlogCommand command)
        {
            Section? section = await _sectionsRepository.GetByIdAsync(command.SectionId);

            if (section is null || !section.IsActive)
            {
                throw new BussinesRuleException("La sección no existe o no está activa.");
            }

            Blog blog = new Blog(command.Name, command.Content, command.SectionId, command.IsPublished);

            try
            {
                Blog created = await _blogsRepository.CreateAsync(blog);
                await _unitOfWork.CommitAsync();
                return created.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
