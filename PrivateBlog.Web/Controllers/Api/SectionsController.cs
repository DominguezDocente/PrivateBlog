using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ApiController
    {
        private readonly ISectionsService _sectionsService;

        public SectionsController(ISectionsService sectionsService)
        {
            _sectionsService = sectionsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<SectionDTO>> response = await _sectionsService.GetPaginationAsync(request);
            return ControllerBasicValidation(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSectionDTO dto)
        {
            Response<CreateSectionDTO> response = await _sectionsService.CreateAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UpdateSectionDTO dto)
        {
            Response<SectionDTO> response = await _sectionsService.UpdateAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response response = await _sectionsService.DeleteAsync(id);
            return ControllerBasicValidation(response);
        }
    }
}
