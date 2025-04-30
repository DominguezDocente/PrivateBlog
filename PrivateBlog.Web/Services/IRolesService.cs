using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public Task<Response<PrivateBlogRoleDTO>> CreateAsync(PrivateBlogRoleDTO dto);
        public Task<Response<PrivateBlogRoleDTO>> EditAsync(PrivateBlogRoleDTO dto);
        public Task<Response<PaginationResponse<PrivateBlogRoleDTO>>> GetListAsync(PaginationRequest request);
        public Task<Response<PrivateBlogRoleDTO>> GetOneAsync(int id);
        public Task<Response<List<PermissionDTO>>> GetPermissionsAsync();
        public Task<Response<List<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);
        public Task<Response<List<SectionDTO>>> GetSectionsAsync();
        public Task<Response<List<SectionForDTO>>> GetSectionsByRoleAsync(int id);
    }

    public class RolesService : CustomQueryableOperations, IRolesService
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
                    // Creación del rol
                    PrivateBlogRole role = _mapper.Map<PrivateBlogRole>(dto);
                    await _context.PrivateBlogRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    int roleId = role.Id;

                    // Inserción de permisos
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

                    // Inserción de secciones
                    List<int> sectionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.SectionIds))
                    {
                        sectionIds = JsonConvert.DeserializeObject<List<int>>(dto.SectionIds);
                    }

                    foreach (int sectionId in sectionIds)
                    {
                        RoleSection rolePermission = new RoleSection
                        {
                            RoleId = roleId,
                            SectionId = sectionId
                        };

                        await _context.RoleSections.AddAsync(rolePermission);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseSuccess(dto, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail(ex);
                }
            }
        }

        public async Task<Response<PrivateBlogRoleDTO>> EditAsync(PrivateBlogRoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail($"El role '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado.");
                }

                // Permisos
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

                // Secciones
                List<int> sectionIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.SectionIds))
                {
                    sectionIds = JsonConvert.DeserializeObject<List<int>>(dto.SectionIds);
                }

                // Eliminación de secciones antiguas
                List<RoleSection> oldRoleSections = await _context.RoleSections.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RoleSections.RemoveRange(oldRoleSections);

                // Inserción de nuevas secciones
                foreach (int sectionId in sectionIds)
                {
                    RoleSection roleSection = new RoleSection
                    {
                        RoleId = dto.Id,
                        SectionId = sectionId
                    };

                    await _context.RoleSections.AddAsync(roleSection);
                }

                // Actualización de Rol
                PrivateBlogRole model = _mapper.Map<PrivateBlogRole>(dto);
                _context.PrivateBlogRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseSuccess(dto, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<PrivateBlogRoleDTO>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<PrivateBlogRole> query = _context.PrivateBlogRoles.AsQueryable();

                if (!string.IsNullOrEmpty(request.Filter))
                {
                    query = query.Where(b => b.Name.Contains(request.Filter));
                }

                return await GetPaginationAsync<PrivateBlogRole, PrivateBlogRoleDTO>(request, query);
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<PrivateBlogRoleDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PrivateBlogRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                PrivateBlogRole? role = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail("El rol con el id indicado no existe");
                }

                List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                    Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id)
                }).ToListAsync();

                List<SectionForDTO> sections = await _context.Sections.Select(p => new SectionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Selected = _context.RoleSections.Any(rs => rs.SectionId == p.Id && rs.RoleId == role.Id)
                }).ToListAsync();

                PrivateBlogRoleDTO dto =  new PrivateBlogRoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = permissions,
                    Sections = sections
                };

                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseSuccess(dto);
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<PermissionDTO>>> GetPermissionsAsync()
        {
            return await GetCompleteListAsync<Permission, PermissionDTO>();
        }

        public async Task<Response<List<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<PrivateBlogRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<List<PermissionForDTO>>.MakeResponseFail(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<List<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<SectionDTO>>> GetSectionsAsync()
        {
            return await GetCompleteListAsync<Section, SectionDTO>();
        }

        public async Task<Response<List<SectionForDTO>>> GetSectionsByRoleAsync(int id)
        {
            try
            {
                Response<PrivateBlogRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<List<SectionForDTO>>.MakeResponseFail(response.Message);
                }

                List<SectionForDTO> sections = response.Result.Sections;

                return ResponseHelper<List<SectionForDTO>>.MakeResponseSuccess(sections);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<SectionForDTO>>.MakeResponseFail(ex);
            }
        }
    }
}
