using PrivateBlog.Application.Utils.Mediator;
using System;

namespace PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection
{
    public class ActivateSectionCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}
