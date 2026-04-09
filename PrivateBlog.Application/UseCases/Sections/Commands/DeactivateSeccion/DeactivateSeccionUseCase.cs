using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSeccion
{
    public class DeactivateSeccionUseCase : IRequestHandler<DeactivateSeccionCommand>
    {
        private ISectionsRepository _repository;
        private IUnitOfWork _unitOfWork;

        public DeactivateSeccionUseCase(ISectionsRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeactivateSeccionCommand command)
        {
            Section? section = await _repository.GetByIdAsync(command.Id);

            if (section is null)
            {
                throw new BussinesRuleException($"No existe sección con id '{command.Id}'");
            }

            try
            {
                section.Deactivate();
                await _repository.UpdateAsync(section);
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
