using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Api.DTOs.Section;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Security;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection;
using PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1,
                                               [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                               [FromQuery] string? nameFilter = null,
                                               [FromQuery] bool? isActiveFilter = null)
        {
            try
            {
                PaginationRequest paginationRequest = new PaginationRequest(page, pageSize);
                GetSectionsListQuery query = new GetSectionsListQuery
                {
                    Pagination = paginationRequest,
                    NameFilter = nameFilter,
                    IsActiveFilter = isActiveFilter
                };

                PaginationResponse<SectionListItemDTO> list = await _mediator.Send(query);

                //return Ok(list);
                return StatusCode(StatusCodes.Status200OK, list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                CreateSectionCommand command = new CreateSectionCommand
                {
                    Name = dto.Name
                };

                Guid newSectionId = await _mediator.Send(command);

                return StatusCode(StatusCodes.Status201Created, newSectionId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] EditSectionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                UpdateSectionCommand command = new UpdateSectionCommand
                {
                    Id = id,
                    Name = dto.Name,
                    IsActive = dto.IsActive
                };

                await _mediator.Send(command);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteSectionCommand { Id = id });
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
