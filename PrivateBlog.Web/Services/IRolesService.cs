using testc = Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using System.Data;

namespace PrivateBlog.Web.Services
{
    public interface IRolesService
    {
        public Task<Response<PrivateBlogRole>> CreateAsync(PrivateBlogRoleDTO dto);
        public Task<Response<PrivateBlogRole>> EditAsync(PrivateBlogRoleDTO dto);
        public Task<Response<PaginationResponse<PrivateBlogRole>>> GetListAsync(PaginationRequest request);
        public Task<Response<PrivateBlogRoleDTO>> GetOneAsync(int id);
        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
        public Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);
    }

    public class RolesService : IRolesService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public RolesService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<PrivateBlogRole>> CreateAsync(PrivateBlogRoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Creación del rol
                    PrivateBlogRole role = _converterHelper.ToRole(dto);
                    await _context.PrivateBlogRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    // Inserción de permisos
                    int roleId = role.Id;

                    List<int> permissionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                    }

                    foreach (int permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ResponseHelper<PrivateBlogRole>.MakeResponseSuccess(role, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseHelper<PrivateBlogRole>.MakeResponseFail(ex);
                }
            }
        }

        public async Task<Response<PrivateBlogRole>> EditAsync(PrivateBlogRoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<PrivateBlogRole>.MakeResponseFail($"El role '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado.");
                }

                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                // Eliminación de permisos antiguos
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Inserción de nuevos permisos
                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                // Actualización de Rol
                PrivateBlogRole model = _converterHelper.ToRole(dto);
                _context.PrivateBlogRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<PrivateBlogRole>.MakeResponseSuccess(model, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivateBlogRole>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<PrivateBlogRole>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<PrivateBlogRole> query = _context.PrivateBlogRoles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<PrivateBlogRole> list = await PagedList<PrivateBlogRole>.ToPagedListAsync(query, request);

                PaginationResponse<PrivateBlogRole> result = new PaginationResponse<PrivateBlogRole>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<PrivateBlogRole>>.MakeResponseSuccess(result, "Roles obtenidos con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<PrivateBlogRole>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PrivateBlogRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                PrivateBlogRole? role = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail("El blog con el id indicado no existe");
                }

                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseSuccess(await _converterHelper.ToRoleDTOAsync(role));
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            try
            {
                IEnumerable<Permission>  permissions = await _context.Permissions.ToListAsync();

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex) 
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<PrivateBlogRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }
    }
}
