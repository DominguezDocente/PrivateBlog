using Microsoft.AspNetCore.Identity;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.DTOs.User;

namespace PrivateBlog.Web.Services.Abstractions
{
    public interface IUsersService
    {
        public Task<Response<IdentityResult>> AddUserAsync(User user, string password);
        public Task<Response<User>> GetUserByEmailAsync(string email);
        public Task<Response<UserDTO>> UpdateUserAsync(UserDTO dto);
        public Task<Response<User>> GetUserByIdAsync(string id);
        public Task<Response<UserDTO>> CreateAsync(CreateUserDTO dto);
        public Task<Response<PaginationResponse<UserDTO>>> GetPaginationAsync(PaginationRequest request);
        public Task<Response<UserDTO>> GetOneAsync(Guid id);
    }
}
