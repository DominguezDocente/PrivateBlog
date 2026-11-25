using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SectionsController : ApiController
    {
        private readonly ISectionsService _sectionsService;

        public SectionsController(ISectionsService sectionsService)
        {
            _sectionsService = sectionsService;
        }

        [HttpGet]
        [ApiAuthorize("sections.show")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<SectionDTO>> response = await _sectionsService.GetPaginationAsync(request);
            return ControllerBasicValidation(response);
        }

        [HttpPost]
        [ApiAuthorize("sections.create")]
        public async Task<IActionResult> Create([FromBody] CreateSectionDTO dto)
        {
            Response<CreateSectionDTO> response = await _sectionsService.CreateAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpPut]
        [ApiAuthorize("sections.edit")]
        public async Task<IActionResult> Edit([FromBody] UpdateSectionDTO dto)
        {
            Response<SectionDTO> response = await _sectionsService.UpdateAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpDelete("{id}")]
        [ApiAuthorize("sections.delete")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response response = await _sectionsService.DeleteAsync(id);
            return ControllerBasicValidation(response);
        }
    }
}
