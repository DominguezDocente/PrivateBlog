using PrivateBlog.Application.Utils.Mediator;
using System;

namespace PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection
{
    public class DeleteSectionCommand : IRequest
    {
        public required Guid Id { get; set; }
    }
}
