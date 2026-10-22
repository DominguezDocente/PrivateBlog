using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.DTOs.User;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUsersService _usersService;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IUsersService usersService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersService = usersService;
        }
        public async Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token)
        {
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            return new Response<IdentityResult>
            {
                Result = result,
                IsSuccess = result.Succeeded
            };
        }

        public async Task<Response<string>> GenerateConfirmationTokenAsync(User user)
        {
            string result = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Response<string>.Success(result);
        }

        public async Task<Response<SignInResult>> LoginAsync(LoginDTO dto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            return new Response<SignInResult>
            {
                Result = result,
                IsSuccess = result.Succeeded
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<AccountUserDTO>> UpdateAccountUserAsync(AccountUserDTO dto)
        {
            try
            {
                // TODO: Validar usuario en sesión

                UserDTO userDto = new UserDTO
                {
                    Id = dto.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Phone = dto.Phone,
                    Photo = dto.Photo,
                    Document = dto.Document
                };

                Response<UserDTO> userDtoResponse = await _usersService.UpdateUserAsync(userDto);

                if (userDtoResponse.IsSuccess)
                {
                    return Response<AccountUserDTO>.Success(dto);
                }

                return Response<AccountUserDTO>.Failure(userDtoResponse.Message, userDtoResponse.Errors);

            }
            catch (Exception ex)
            {
                return Response<AccountUserDTO>.Failure(ex);
            }
        }
    }
}
