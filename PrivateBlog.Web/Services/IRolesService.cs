using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;

namespace PrivateBlog.Web.Services
{
    public interface IRolesService
    {
        public Task<Response<PrivateBlogRole>> CreateAsync(PrivateBlogRoleDTO dto);

        public Task<Response<object>> DeleteAsync(int id);

        public Task<Response<PrivateBlogRole>> EditAsync(PrivateBlogRoleDTO dto);

        public Task<Response<PaginationResponse<PrivateBlogRole>>> GetListAsync(PaginationRequest request);

        public Task<Response<PrivateBlogRoleDTO>> GetOneAsync(int id);

        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();

        public Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);

        public Task<Response<IEnumerable<Section>>> GetSectionsAsync();

        public Task<Response<IEnumerable<SectionForDTO>>> GetSectionsByRoleAsync(int id);
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
                    // Creación de Rol
                    PrivateBlogRole model = _converterHelper.ToRole(dto);
                    EntityEntry<PrivateBlogRole> modelStored = await _context.PrivateBlogRoles.AddAsync(model);

                    await _context.SaveChangesAsync();

                    // Inserción de permisos
                    int roleId = modelStored.Entity.Id;

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

                        _context.RolePermissions.Add(rolePermission);
                    }

                    // Inserción de secciones
                    List<int> sectionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.SectionIds))
                    {
                        sectionIds = JsonConvert.DeserializeObject<List<int>>(dto.SectionIds);
                    }

                    foreach (int sectionId in sectionIds)
                    {
                        RoleSection roleSection = new RoleSection
                        {
                            RoleId = roleId,
                            SectionId = sectionId
                        };

                        _context.RoleSections.Add(roleSection);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return ResponseHelper<PrivateBlogRole>.MakeResponseSuccess("Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseHelper<PrivateBlogRole>.MakeResponseFail(ex);
                }
            }
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            try
            {
                Response<PrivateBlogRole> roleResponse = await GetOneModelAsync(id);

                if (!roleResponse.IsSuccess)
                {
                    return ResponseHelper<object>.MakeResponseFail(roleResponse.Message);
                }

                if (roleResponse.Result.Name == Constants.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<object>.MakeResponseFail($"El rol {Constants.SUPER_ADMIN_ROLE_NAME} no puede ser eliminado");
                }

                if (roleResponse.Result.Users.Count() > 0)
                {
                    return ResponseHelper<object>.MakeResponseFail($"El rol no puede ser eliminado debido a que tiene usuarios relacionados");
                }

                _context.PrivateBlogRoles.Remove(roleResponse.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<object>.MakeResponseSuccess("Rol eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PrivateBlogRole>> EditAsync(PrivateBlogRoleDTO dto)
        {
            try
            {
                if (dto.Name == Constants.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<PrivateBlogRole>.MakeResponseFail($"El Rol '{Constants.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                // Permisos
                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                // Eliminación de antiguos permisos
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rs => rs.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Inserción de nuevos permisos
                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    _context.RolePermissions.Add(rolePermission);
                }

                // Secciones
                List<int> sectionIds = new List<int>();

                if (!string.IsNullOrEmpty(dto.SectionIds))
                {
                    sectionIds = JsonConvert.DeserializeObject<List<int>>(dto.SectionIds);
                }

                // Eliminación de antiguos permisos
                List<RoleSection> oldRoleSections = await _context.RoleSections.Where(rs => rs.RoleId == dto.Id).ToListAsync();
                _context.RoleSections.RemoveRange(oldRoleSections);

                // Inserción de nuevos permisos
                foreach (int sectionId in sectionIds)
                {
                    RoleSection roleSection = new RoleSection
                    {
                        RoleId = dto.Id,
                        SectionId = sectionId
                    };

                    _context.RoleSections.Add(roleSection);
                }

                // Actualización de rol
                PrivateBlogRole model = _converterHelper.ToRole(dto);
                _context.PrivateBlogRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<PrivateBlogRole>.MakeResponseSuccess("Rol editado con éxito");
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
                IQueryable<PrivateBlogRole> queryable = _context.PrivateBlogRoles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    queryable = queryable.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<PrivateBlogRole> list = await PagedList<PrivateBlogRole>.ToPagedListAsync(queryable, request);

                PaginationResponse<PrivateBlogRole> result = new PaginationResponse<PrivateBlogRole>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };

                return ResponseHelper<PaginationResponse<PrivateBlogRole>>.MakeResponseSuccess(result, "Roles obtenidas con éxito");
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
                PrivateBlogRole? privateBlogRole = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Id == id);

                if (privateBlogRole is null)
                {
                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail($"El Rol con id '{id}' no existe.");
                }

                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseSuccess(await _converterHelper.ToRoleDTOAsync(privateBlogRole));
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
                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();

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
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Section>>> GetSectionsAsync()
        {
            try
            {
                IEnumerable<Section> sections = await _context.Sections.Where(s => !s.IsHidden)
                                                                       .ToListAsync();

                return ResponseHelper<IEnumerable<Section>>.MakeResponseSuccess(sections);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Section>>.MakeResponseFail(ex);
            }
        }

        public Task<Response<IEnumerable<SectionForDTO>>> GetSectionsByRoleAsync(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<Response<PrivateBlogRole>> GetOneModelAsync(int id)
        {
            try
            {
                PrivateBlogRole? role = await _context.PrivateBlogRoles.Include(r => r.Users)
                                                                       .FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<PrivateBlogRole>.MakeResponseFail($"El Rol con id '{id}' no existe");
                }

                return ResponseHelper<PrivateBlogRole>.MakeResponseSuccess(role);
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivateBlogRole>.MakeResponseFail(ex);
            }
        }
    }
}