using PrivateBlog.Application.Utils.Mediator;
using System;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSection
{
    public class DeactivateSectionCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}
