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
        public Task<Response<PaginationResponse<PrivateBlogRoleDTO>>> GetPaginationAsync(PaginationRequest request);
        public Task<Response<PrivateBlogRoleDTO>> GetOneAsync(int id);
        public Task<Response<List<PermissionDTO>>> GetPermissionsAsync();
        public Task<Response<List<SectionDTO>>> GetSectionsAsync();
        public Task<Response<PrivateBlogRoleDTO>> CreateAsync(PrivateBlogRoleDTO dto);
        public Task<Response<PrivateBlogRoleDTO>> EditAsync(PrivateBlogRoleDTO dto);
        public Task<Response<List<PermissionForRoleDTO>>> GetPermissionsByRoleAsync(int id);
        public Task<Response<List<SectionForRoleDTO>>> GetSectionsByRoleAsync(int id);
    }

    public class RolesService : CustomQueryableOperations, IRolesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesService(DataContext context, IMapper mapper) : base (context, mapper)
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
                    // Role Creation
                    PrivateBlogRole role = _mapper.Map<PrivateBlogRole>(dto);
                    await _context.PrivateBlogRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    int roleId = role.Id;

                    // Permissions
                    List<int> permissionIds = new();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                    }

                    foreach(int permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                    // Sections
                    List<int> sectionIds = new();

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
                catch(Exception ex)
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
                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail($"El rol '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                // Permissions
                List<int> permissionIds = new();

                if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                // Delete old permissions
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                // Sections
                List<int> sectionIds = new();

                if (!string.IsNullOrWhiteSpace(dto.SectionIds))
                {
                    sectionIds = JsonConvert.DeserializeObject<List<int>>(dto.SectionIds);
                }

                // Delete old sections
                List<RoleSection> oldRoleSections = await _context.RoleSections.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                foreach (int sectionId in sectionIds)
                {
                    RoleSection rolePermission = new RoleSection
                    {
                        RoleId = dto.Id,
                        SectionId = sectionId
                    };

                    await _context.RoleSections.AddAsync(rolePermission);
                }

                // Update role
                PrivateBlogRole role = _mapper.Map<PrivateBlogRole>(dto);
                _context.PrivateBlogRoles.Update(role);

                await _context.SaveChangesAsync();

                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseSuccess(dto, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail(ex);
            }
            
        }

        public async Task<Response<PaginationResponse<PrivateBlogRoleDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<PrivateBlogRole> query = _context.PrivateBlogRoles.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(b => b.Name.ToLower()
                                               .Contains(request.Filter
                                               .ToLower()));
            }

            return await GetPaginationAsync<PrivateBlogRole, PrivateBlogRoleDTO>(request, query);
        }

        public async Task<Response<PrivateBlogRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                PrivateBlogRole role = await _context.PrivateBlogRoles.FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail($"El rol con id '{id}' no existe.");
                }

                List<PermissionForRoleDTO> permissions = await _context.Permissions.Select(p => new PermissionForRoleDTO
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    Module = p.Module,
                    Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id)
                }).ToListAsync();


                List<SectionForRoleDTO> sections = await _context.Sections.Select(p => new SectionForRoleDTO
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    Selected = _context.RoleSections.Any(rs => rs.SectionId == p.Id && rs.RoleId == role.Id)
                }).ToListAsync();

                PrivateBlogRoleDTO dto = new PrivateBlogRoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = permissions,
                    Sections = sections
                };

                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseSuccess(dto);
            }
            catch(Exception ex)
            {
                return ResponseHelper<PrivateBlogRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<PermissionDTO>>> GetPermissionsAsync()
        {
            return await GetCompleteList<Permission, PermissionDTO>();
        }

        public async Task<Response<List<SectionDTO>>> GetSectionsAsync()
        {
            return await GetCompleteList<Section, SectionDTO>();
        }

        public async Task<Response<List<PermissionForRoleDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<PrivateBlogRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<List<PermissionForRoleDTO>>.MakeResponseFail(response.Message);
                }

                List<PermissionForRoleDTO> permissions = response.Result.Permissions;

                return ResponseHelper<List<PermissionForRoleDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<PermissionForRoleDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<SectionForRoleDTO>>> GetSectionsByRoleAsync(int id)
        {
            try
            {
                Response<PrivateBlogRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<List<SectionForRoleDTO>>.MakeResponseFail(response.Message);
                }

                List<SectionForRoleDTO> sections = response.Result.Sections;

                return ResponseHelper<List<SectionForRoleDTO>>.MakeResponseSuccess(sections);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<SectionForRoleDTO>>.MakeResponseFail(ex);
            }
        }
    }
}
