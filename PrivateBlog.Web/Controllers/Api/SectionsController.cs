using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

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
        public async Task<IActionResult> GetSections([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<SectionDTO>> response = await _sectionsService.GetPaginationAsync(request);
            return ControllerBasicValidation(response);
        }

        [HttpGet("{id:int}")]
        [ApiAuthorize(permission: "showSections", module: "Secciones")]
        public async Task<IActionResult> GetOneSection([FromRoute] int id)
        {
            return ControllerBasicValidation(await _sectionsService.GetOneAsync(id));
        }

        [HttpPost]
        [ApiAuthorize(permission: "createSections", module: "Secciones")]
        public async Task<IActionResult> CreateSection([FromBody] SectionDTO dto)
        {
            return ControllerBasicValidation(await _sectionsService.CreateAsync(dto), ModelState);
        }

        [HttpPut]
        [ApiAuthorize(permission: "editSections", module: "Secciones")]
        public async Task<IActionResult> EditSection([FromBody] SectionDTO dto)
        {
            return ControllerBasicValidation(await _sectionsService.EditAsync(dto), ModelState);
        }

        [HttpDelete("{id:int}")]
        [ApiAuthorize(permission: "deleteSections", module: "Secciones")]
        public async Task<IActionResult> DeleteSection([FromRoute] int id)
        {
            return ControllerBasicValidation(await _sectionsService.DeleteAsync(id), ModelState);
        }
    }
}
