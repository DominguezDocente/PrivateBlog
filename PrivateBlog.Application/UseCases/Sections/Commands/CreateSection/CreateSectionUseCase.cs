using PrivateBlog.Application.Contracts.Persistence;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Application.UseCases.Sections.Commands.CreateSection
{
    public class CreateSectionUseCase : IRequestHandler<CreateSectionCommand, Guid>
    {
        private readonly ISectionsRepository _sectionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSectionUseCase(ISectionsRepository sectionsRepository, IUnitOfWork unitOfWork)
        {
            _sectionsRepository = sectionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSectionCommand command)
        {
            Section section = new Section(command.Name);

            try
            {
                Section response = await _sectionsRepository.CreateAsync(section);
                await _unitOfWork.CommitAsync();
                return response.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
