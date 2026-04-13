using PrivateBlog.Application.Contracts.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection
{
    public class DeleteSectionUseCase : IRequestHandler<DeleteSectionCommand>
    {
        private readonly ISectionsRepository _sectionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSectionUseCase(ISectionsRepository sectionsRepository, IUnitOfWork unitOfWork)
        {
            _sectionsRepository = sectionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSectionCommand command)
        {
            Section? section = await _sectionsRepository.GetByIdAsync(command.Id);

            if (section is null)
            {
                throw new BusinessRuleException("La sección no existe.");
            }

            try
            {
                await _sectionsRepository.DeleteAsync(section);
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
