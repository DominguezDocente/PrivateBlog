using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [ApiAuthorize(permission: "showSections", module: "Secciones")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            return ControllerBasicValidation(await _sectionsService.GetPaginationAsync(request));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Response<SectionDTO> response = await _sectionsService.CreateAsync(dto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
