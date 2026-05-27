using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Api.DTOs.Sections;
using PrivateBlog.Api.Security;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Security;
using PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSeccion;
using PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection;
using PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [RequirePermission(PermissionCodesCatalog.SHOW_SECTIONS)]
        public async Task<ActionResult<PaginationResponse<SectionListItemDTO>>> GetList(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
            [FromQuery] string? nameFilter = null,
            [FromQuery] bool? isActiveFilter = null)
        {
            PaginationRequest paginationRequest = new PaginationRequest(page, pageSize);
            GetSectionsListQuery query = new GetSectionsListQuery
            {
                Pagination = paginationRequest,
                NameFilter = nameFilter,
                IsActiveFilter = isActiveFilter
            };

            PaginationResponse<SectionListItemDTO> list = await _mediator.Send(query);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        [RequirePermission(PermissionCodesCatalog.SHOW_SECTIONS)]
        public async Task<ActionResult<SectionDetailDTO>> GetById([FromRoute] Guid id)
        {
            SectionDetailDTO dto = await _mediator.Send(new GetSectionByIdQuery { Id = id });
            return Ok(dto);
        }

        [HttpPost]
        [RequirePermission(PermissionCodesCatalog.CREATE_SECTIONS)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateSectionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            CreateSectionCommand command = new CreateSectionCommand { Name = request.Name };
            Guid newSectionId = await _mediator.Send(command);
            return Created($"/api/sections/{newSectionId}", newSectionId);
        }

        [HttpPut("{id:guid}")]
        [RequirePermission(PermissionCodesCatalog.EDIT_SECTIONS)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSectionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            UpdateSectionCommand command = new UpdateSectionCommand
            {
                Id = id,
                Name = request.Name,
                IsActive = request.IsActive
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission(PermissionCodesCatalog.DELETE_SECTIONS)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteSectionCommand { Id = id });
            return NoContent();
        }

        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> Activate([FromRoute] Guid id)
        {
            await _mediator.Send(new ActivateSectionCommand { Id = id });
            return NoContent();
        }

        [HttpPost("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id)
        {
            await _mediator.Send(new DeactivateSeccionCommand { Id = id });
            return NoContent();
        }
    }
}
