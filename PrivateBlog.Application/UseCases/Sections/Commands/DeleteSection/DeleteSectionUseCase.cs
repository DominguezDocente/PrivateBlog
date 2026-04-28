using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection
{
    public class DeleteSectionUseCase : IRequestHandler<DeleteSectionCommand>
    {
        private ISectionsRepository _repository;
        private IUnitOfWork _unitOfWork;

        public DeleteSectionUseCase(ISectionsRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSectionCommand command)
        {
            Section? section = await _repository.GetByIdAsync(command.Id);

            if (section is null)
            {
                throw new BussinesRuleException($"No existe sección con id '{command.Id}'");
            }

            if (await _repository.HasArticlesAsync(command.Id))
            {
                throw new BussinesRuleException($"La sección con id '{command.Id}' tiene artículos asociados.");
            }

            try
            {
                await _repository.DeleteAsync(section);
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
