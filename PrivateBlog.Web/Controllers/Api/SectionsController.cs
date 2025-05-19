using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionsService _sectionsService;

        public SectionsController(ISectionsService sectionsService)
        {
            _sectionsService = sectionsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {

            Response<PaginationResponse<SectionDTO>> response = await _sectionsService.GetPaginationAsync(request);
            return Ok(response);
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
