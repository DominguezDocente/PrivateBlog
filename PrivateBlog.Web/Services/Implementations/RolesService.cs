using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateRole.Web.Services.Abtractions;

namespace PrivateBlog.Web.Services.Implementations
{
    public class RolesService : CustomQueryableOperationsService, IRolesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PrivateBlogRoleDTO>> CreateAsync(PrivateBlogRoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Guid newRoleId = Guid.NewGuid();

                    // Role
                    PrivateBlogRole role = _mapper.Map<PrivateBlogRole>(dto);

                    await _context.PrivateBlogRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    // Permissions
                    List<Guid> permissionIds = new();

                    if (!string.IsNullOrEmpty(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                    }

                    foreach(Guid permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            PrivateBlogRoleId = newRoleId,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Response<PrivateBlogRoleDTO>.Success(dto, "Rol creado con éxito");
                } 
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return Response<PrivateBlogRoleDTO>.Failure(ex);
                }
            }
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            return await DeleteAsync<PrivateBlogRole>(id);
        }

        public async Task<Response<PrivateBlogRoleDTO>> EditAsync(PrivateBlogRoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return Response<PrivateBlogRoleDTO>.Failure($"El rol '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                // Role
                PrivateBlogRole role = _mapper.Map<PrivateBlogRole>(dto);
                _context.PrivateBlogRoles.Update(role);

                // Permissions
                List<Guid> permissionIds = new();

                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                }

                // Delete old
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.PrivateBlogRoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Create new ones
                foreach (Guid permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        PrivateBlogRoleId = role.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                await _context.SaveChangesAsync();

                return Response<PrivateBlogRoleDTO>.Success(dto, "Rol actualizado con éxito");
            }
            catch(Exception ex)
            {
                return Response<PrivateBlogRoleDTO>.Failure(ex);
            }
        }

        public async Task<Response<PrivateBlogRoleDTO>> GetOneAsync(Guid id)
        {
            Response<PrivateBlogRoleDTO> response = await GetOneAsync<PrivateBlogRole, PrivateBlogRoleDTO>(id);

            if (!response.IsSuccess)
            {
                return response;
            }

            PrivateBlogRoleDTO dto = response.Result;

            List<PermissionsForRoleDTO> permissions = await _context.Permissions.Select(p => new PermissionsForRoleDTO
            {
                Id = p.Id,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.PrivateBlogRoleId == dto.Id)
            }).ToListAsync();

            dto.Permissions = permissions;

            return Response<PrivateBlogRoleDTO>.Success(dto, "Rol obtenido con éxito");
        }

        public async Task<Response<PaginationResponse<PrivateBlogRoleDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<PrivateBlogRole> query = _context.PrivateBlogRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(r => r.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<PrivateBlogRole, PrivateBlogRoleDTO>(request, query);
        }

        public async Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync()
        {
            Response<List<PermissionDTO>> permissionsResponse = await GetCompleteListAsync<Permission, PermissionDTO>();

            if (!permissionsResponse.IsSuccess)
            {
                return Response<List<PermissionsForRoleDTO>>.Failure(permissionsResponse.Message);
            }

            List<PermissionsForRoleDTO> dto = permissionsResponse.Result.Select(p => new PermissionsForRoleDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = false
            }).ToList();

            return Response<List<PermissionsForRoleDTO>>.Success(dto);
        }
    }
}
