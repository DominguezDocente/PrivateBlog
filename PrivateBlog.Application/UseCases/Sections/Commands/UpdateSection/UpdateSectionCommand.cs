using PrivateBlog.Application.Utils.Mediator;
using System;

namespace PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection
{
    public class UpdateSectionCommand : IRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
