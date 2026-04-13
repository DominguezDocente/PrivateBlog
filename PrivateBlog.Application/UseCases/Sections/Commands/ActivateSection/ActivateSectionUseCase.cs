using PrivateBlog.Application.Contracts.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection
{
    public class ActivateSectionUseCase : IRequestHandler<ActivateSectionCommand>
    {
        private readonly ISectionsRepository _sectionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateSectionUseCase(ISectionsRepository sectionsRepository, IUnitOfWork unitOfWork)
        {
            _sectionsRepository = sectionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ActivateSectionCommand command)
        {
            Section? section = await _sectionsRepository.GetByIdAsync(command.Id);

            if (section is null)
            {
                throw new BusinessRuleException("La sección no existe.");
            }

            section.Activate();

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
