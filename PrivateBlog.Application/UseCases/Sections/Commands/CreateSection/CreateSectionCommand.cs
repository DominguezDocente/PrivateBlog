using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Sections.Commands.CreateSection
{
    public class CreateSectionCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
    }
}
