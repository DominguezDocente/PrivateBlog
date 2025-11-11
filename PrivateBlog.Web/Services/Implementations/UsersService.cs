using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services.Abtractions;
using System.Security.Claims;

namespace PrivateBlog.Web.Services.Implementations
{
    // TODO: Mejorar mensajes de error
    public class UsersService : CustomQueryableOperationsService, IUsersService
    {
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UsersService(DataContext context,
                            SignInManager<User> signInManager,
                            UserManager<User> userManager, 
                            IHttpContextAccessor httpContextAccessor,
                            IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<Response<IdentityResult>> AddUserAsync(User user, string password)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password);

            return new Response<IdentityResult>
            {
                Result = result,
                IsSuccess = result.Succeeded,
            };
        }

        public async Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token)
        {
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            return new Response<IdentityResult>
            {
                Result = result,
                IsSuccess = result.Succeeded,
            };
        }

        public async Task<Response<UserDTO>> CreateAsync(UserDTO dto)
        {
            try
            {
                User user = _mapper.Map<User>(dto);
                user.Id = Guid.NewGuid().ToString();

                Response<IdentityResult> response = await AddUserAsync(user, dto.Document);

                // TODO: Envío de email para confirmación
                Response<string> token = await GenerateConfirmationTokenAsync(user);

                await ConfirmUserAsync(user, token.Result);

                return Response<UserDTO>.Success(_mapper.Map<UserDTO>(user), "Usuario creado con éxito");
            } 
            catch(Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }

        public bool CurrentUserIsAuthenticaded()
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity is not null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {
            ClaimsPrincipal? claimsUser = _httpContextAccessor.HttpContext?.User;

            // Valida si hay sesión
            if (claimsUser is null)
            {
                return false;
            }

            string userName = claimsUser.Identity!.Name!;

            User? user = await GetUserByEmailAsync(userName);

            if (user is null)
            {
                return false;
            }

            if (user.PrivateBlogRole.Name == Env.SUPER_ADMIN_ROLE_NAME)
            {
                return true;
            }

            return await _context.Permissions.Include(p => p.RolePermissions)
                                             .AnyAsync(p => (p.Module == module && p.Name == permission)
                                                            && p.RolePermissions.Any(rp => rp.PrivateBlogRoleId == user.PrivateBlogRoleId));
        }

        //public Task<Response<object>> DeleteAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Response<UserDTO>> EditAsync(UserDTO dto)
        {
            try
            {
                User? user = await GetUserAsync(dto.Id.ToString());    

                if (user == null)
                {
                    return Response<UserDTO>.Failure("No existe usuario.");
                }

                user.PhoneNumber = dto.PhoneNumber;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PrivateBlogRoleId = Guid.Parse(dto.PrivateBlogRoleId);

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return Response<UserDTO>.Success(dto, "Usuario actualizado con éxito");
            } 
            catch(Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }

        public async Task<Response<string>> GenerateConfirmationTokenAsync(User user)
        {
            string result = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Response<string>.Success(result);
        }

        //public Task<Response<UserDTO>> GetOneAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Response<PaginationResponse<UserDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<User> queryable = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                queryable = queryable.Where(u => u.FirstName.ToLower().Contains(request.Filter.ToLower())
                                                || u.LastName.ToLower().Contains(request.Filter.ToLower())
                                                || u.Document.ToLower().Contains(request.Filter.ToLower())
                                                || u.Email.ToLower().Contains(request.Filter.ToLower())
                                                || u.PhoneNumber.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<User, UserDTO>(request, queryable);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.PrivateBlogRole)
                                       .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.PrivateBlogRole)
                                       .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<Response<SignInResult>> LoginAsync(LoginDTO dto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            return new Response<SignInResult>
            {
                Result = result,
                IsSuccess = result.Succeeded,
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<AccountUserDTO>> UpdateUserAsync(AccountUserDTO dto)
        {
            try
            {
                User user = await GetUserAsync(dto.Id);
                user.PhoneNumber = dto.PhoneNumber;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return Response<AccountUserDTO>.Success(dto, "Datos actualizados con éxito");
            }
            catch(Exception ex)
            {
                return Response<AccountUserDTO>.Failure(ex);
            }
        }

        private async Task<User> GetUserAsync(string? id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
