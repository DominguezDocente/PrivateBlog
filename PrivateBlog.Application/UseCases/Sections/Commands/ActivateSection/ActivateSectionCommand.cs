using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection
{
    public class ActivateSectionCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}
