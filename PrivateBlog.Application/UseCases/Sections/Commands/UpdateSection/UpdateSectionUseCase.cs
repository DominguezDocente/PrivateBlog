using PrivateBlog.Application.Contracts.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection
{
    public class UpdateSectionUseCase : IRequestHandler<UpdateSectionCommand>
    {
        private readonly ISectionsRepository _sectionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSectionUseCase(ISectionsRepository sectionsRepository, IUnitOfWork unitOfWork)
        {
            _sectionsRepository = sectionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateSectionCommand command)
        {
            Section? section = await _sectionsRepository.GetByIdAsync(command.Id);

            if (section is null)
            {
                throw new BussinesRuleException("La sección no existe.");
            }

            section.UpdateName(command.Name);

            if (command.IsActive)
            {
                section.Activate();
            }
            else
            {
                section.Deactivate();
            }

            try
            {
                await _sectionsRepository.UpdateAsync(section);
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
