using PrivateBlog.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Sections.Commands.CreateSection
{
    public class CreateSectionCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
    }
}
