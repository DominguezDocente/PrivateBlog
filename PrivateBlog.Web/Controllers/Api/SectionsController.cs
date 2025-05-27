using Microsoft.AspNetCore.Authentication;
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

        [HttpGet]
        [ApiAuthorize(permission: "showSections", module: "Secciones")]
        public async Task<IActionResult> GetSections([FromQuery] PaginationRequest request)
        {
            return ControllerBasicValidation(await _sectionsService.GetPaginationAsync(request));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody] SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            Response<SectionDTO> response = await _sectionsService.CreateAsync(dto);

            if (response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created, response);
            }

            return StatusCode(StatusCodes.Status400BadRequest, response);
        }
    }
}
