using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection
{
    public class UpdateSectionCommand : IRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool IsActive { get; set; }
    }
}
