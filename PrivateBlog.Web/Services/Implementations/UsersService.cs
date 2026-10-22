using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.DTOs.User;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public UsersService(UserManager<User> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Response<IdentityResult>> AddUserAsync(User user, string password)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password);

            return new Response<IdentityResult>
            {
                Result = result,
                IsSuccess = result.Succeeded
            };
        }

        public async Task<Response<User>> GetUserByEmailAsync(string email)
        {
            try
            {
                User? user = await _context.Users.Include(u => u.PrivateBlogRole)
                                                 .FirstOrDefaultAsync(u => u.Email == email);

                if (user is null)
                {
                    return Response<User>.Failure($"No existe usuario con email: '{email}'");
                }

                return Response<User>.Success(user);
            }
            catch(Exception ex)
            {
                return Response<User>.Failure(ex);
            }
        }

        public async Task<Response<User>> GetUserByIdAsync(string id)
        {
            try
            {
                User? user = await _context.Users.Include(u => u.PrivateBlogRole)
                                                 .FirstOrDefaultAsync(u => u.Id == id);

                if (user is null)
                {
                    return Response<User>.Failure($"No existe usuario con id: '{id}'");
                }

                return Response<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure(ex);
            }
        }

        public async Task<Response<UserDTO>> UpdateUserAsync(UserDTO dto)
        {
            try
            {
                Response<User> response = await GetUserByIdAsync(dto.Id.ToString());

                if (!response.IsSuccess)
                {
                    return Response<UserDTO>.Failure(response.Message, response.Errors);
                }

                User user = response.Result;
                user.PhoneNumber = dto.Phone;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Photo = dto.Photo;
                user.PrivateBlogRoleId = dto.PrivateBlogRoleId == Guid.Empty ? user.PrivateBlogRoleId : dto.PrivateBlogRoleId;
                

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return Response<UserDTO>.Success(dto, "Usuario actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }
    }
}
