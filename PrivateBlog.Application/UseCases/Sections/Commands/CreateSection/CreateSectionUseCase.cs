using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.CreateSection
{
    public class CreateSectionUseCase : IRequestHandler<CreateSectionCommand, Guid>
    {
        private readonly ISectionsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSectionUseCase(ISectionsRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSectionCommand command)
        {
            Section section = new Section(command.Name);

            try
            {
                Section newSection = await _repository.CreateAsync(section);
                await _unitOfWork.CommitAsync();
                return newSection.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
