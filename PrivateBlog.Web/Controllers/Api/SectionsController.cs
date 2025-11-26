using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services.Abtractions;

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
        [ApiAuthorize(permission: "showSections", module: "Secciones")]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<SectionDTO>> response = await _sectionsService.GetPaginatedListAsync(request);
            return ControllerBasicValidation(response);
        }

        [HttpGet("{id}")]
        [ApiAuthorize(permission: "showSections", module: "Secciones")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            Response<SectionDTO> response = await _sectionsService.GetOneAsync(id);
            return ControllerBasicValidation(response);
        }

        [HttpPost]
        [ApiAuthorize(permission: "createSections", module: "Secciones")]
        public async Task<IActionResult> Create([FromBody] SectionDTO dto)
        {
            Response<SectionDTO> response = await _sectionsService.CreateAsync(dto);
            return ControllerBasicValidation(response, ModelState);
        }

        [HttpPut]
        [ApiAuthorize(permission: "updateSections", module: "Secciones")]
        public async Task<IActionResult> Edit([FromBody] SectionDTO dto)
        {
            Response<SectionDTO> response = await _sectionsService.EditAsync(dto);
            return ControllerBasicValidation(response, ModelState);
        }

        [HttpDelete("{id}")]
        [ApiAuthorize(permission: "deleteSections", module: "Secciones")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _sectionsService.DeleteAsync(id);
            return ControllerBasicValidation(response);
        }
    }
}
