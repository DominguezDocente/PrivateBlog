using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection
{
    public class DeleteSectionCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}
