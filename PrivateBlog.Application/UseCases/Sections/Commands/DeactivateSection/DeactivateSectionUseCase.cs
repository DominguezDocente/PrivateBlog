using PrivateBlog.Application.Contracts.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSection
{
    public class DeactivateSectionUseCase : IRequestHandler<DeactivateSectionCommand>
    {
        private readonly ISectionsRepository _sectionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateSectionUseCase(ISectionsRepository sectionsRepository, IUnitOfWork unitOfWork)
        {
            _sectionsRepository = sectionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeactivateSectionCommand command)
        {
            Section? section = await _sectionsRepository.GetByIdAsync(command.Id);

            if (section is null)
            {
                throw new BusinessRuleException("La sección no existe.");
            }

            section.Deactivate();

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
