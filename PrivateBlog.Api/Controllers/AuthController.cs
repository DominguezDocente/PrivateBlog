using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Api.DTOs.Auth;
using PrivateBlog.Api.Services;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using PrivateBlog.Application.UseCases.Account.Commands.Logout;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo;
using PrivateBlog.Application.Utilities.Mediator;
using System.Security.Claims;

namespace PrivateBlog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IMediator mediator, IJwtTokenService jwtTokenService)
        {
            _mediator = mediator;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            LoginCommand command = new LoginCommand
            {
                UserName = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe,
                UseCookieAuth = request.UseCookie
            };

            AccountSignInResult result = await _mediator.Send(command);

            if (result.IsLockedOut)
            {
                return StatusCode(StatusCodes.Status423Locked, new
                {
                    message = "Su cuenta ha sido bloqueada temporalmente. Inténtelo más tarde.",
                    isLockedOut = true
                });
            }

            if (result.InvalidCredentials || string.IsNullOrEmpty(result.UserId))
            {
                return Unauthorized(new
                {
                    message = "Usuario o contraseña incorrectos.",
                    isLockedOut = false
                });
            }

            UserAccountInfoDTO userInfo = await _mediator.Send(new GetAccountUserInfoQuery { UserId = result.UserId });

            if (request.UseCookie)
            {
                return Ok(new
                {
                    message = "Inicio de sesión con cookie exitoso.",
                    userId = result.UserId,
                    fullName = userInfo.FullName,
                    roleName = userInfo.RoleName
                });
            }

            JwtTokenResult token = await _jwtTokenService.CreateTokenAsync(result.UserId);

            return Ok(new LoginResponse
            {
                AccessToken = token.AccessToken,
                ExpiresAtUtc = token.ExpiresAtUtc,
                UserId = result.UserId,
                Email = request.Email,
                FullName = userInfo.FullName,
                RoleName = userInfo.RoleName
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new LogoutCommand());
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<AccountProfileDTO>> Me()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            AccountProfileDTO profile = await _mediator.Send(new GetAccountProfileQuery { UserId = userId });
            return Ok(profile);
        }
    }
}
