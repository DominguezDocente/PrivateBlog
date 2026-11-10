using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Role;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Services.Abstractions;

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

        public async Task<Response<RoleDTO>> CreateAsync(RoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Role
                    PrivateBlogRole role = _mapper.Map<PrivateBlogRole>(dto);

                    await _context.PrivateBlogRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    // Permissions
                    List<Guid> permissionIds = [];

                    if (!string.IsNullOrEmpty(dto.PermissionIds))
                    {
                        //permissionIds = dto.PermissionIds.Split(',').Select(Guid.Parse).ToList();
                        permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                    }

                    foreach(Guid permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            PrivateBlogRoleId = role.Id,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                    // Sections
                    List<Guid> sectionIds = [];

                    if (!string.IsNullOrEmpty(dto.SectionIds))
                    {
                        sectionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.SectionIds);
                    }

                    foreach (Guid sectionId in sectionIds)
                    {
                        RoleSection roleSection = new RoleSection
                        {
                            PrivateBlogRoleId = role.Id,
                            SectionId = sectionId
                        };

                        await _context.RoleSections.AddAsync(roleSection);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Response<RoleDTO>.Success(dto, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Response<RoleDTO>.Failure(ex);
                }
            }
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            if (await _context.Users.AnyAsync(u => u.PrivateBlogRoleId == id))
            {
                return Response.Failure("No se puede eliminar el rol porque hay usuarios asociados a él.");
            }

            return await DeleteAsync<PrivateBlogRole>(id);

        }

        public async Task<Response<RoleDTO>> GetOneAsync(Guid id)
        {
            Response<RoleDTO> response = await GetOneAsync<RoleDTO,PrivateBlogRole>(id);

            if (!response.IsSuccess)
            {
                return response;
            }

            RoleDTO dto = response.Result!;

            List<PermissionsForRoleDTO> permissions = await _context.Permissions.Select(p => new PermissionsForRoleDTO 
            {
                Id = p.Id,
                Description = p.Description,
                Module = p.Module,
                Code = p.Code,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.PrivateBlogRoleId == dto.Id)
            }).ToListAsync();

            List<SectionsForRoleDTO> sections = await _context.Sections.Where(s => !s.IsHidden)
                                                                       .Select(s => new SectionsForRoleDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Selected = _context.RoleSections.Any(rs => rs.SectionId == s.Id && rs.PrivateBlogRoleId == dto.Id)
            }).ToListAsync();

            dto.Permissions = permissions;
            dto.Sections = sections;

            return Response<RoleDTO>.Success(dto);
        }

        public async Task<Response<PaginationResponse<RoleDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<PrivateBlogRole> query = _context.PrivateBlogRoles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(r => r.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPagedListAsync<RoleDTO, PrivateBlogRole>(request, query);
        }

        public async Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync()
        {
            Response<List<PermissionDTO>> permissionsResponse = await GetCompleteListAsync<PermissionDTO, Permission>();

            if (!permissionsResponse.IsSuccess)
            {
                return Response<List<PermissionsForRoleDTO>>.Failure(permissionsResponse.Message, permissionsResponse.Errors);
            }

            List<PermissionsForRoleDTO> permissions = permissionsResponse.Result.Select(p => new PermissionsForRoleDTO
            {
                Id = p.Id,
                Description = p.Description,
                Module = p.Module,
                Code = p.Code,
                Selected = false,
            }).ToList();

            return Response<List<PermissionsForRoleDTO>>.Success(permissions);
        }

        public async Task<Response<List<SectionsForRoleDTO>>> GetSectionsAsync()
        {
            Response<List<SectionDTO>> sectionsResponse = await GetCompleteListAsync<SectionDTO, Section>();

            if (!sectionsResponse.IsSuccess)
            {
                return Response<List<SectionsForRoleDTO>>.Failure(sectionsResponse.Message, sectionsResponse.Errors);
            }

            List<SectionsForRoleDTO> sections = sectionsResponse.Result.Where(s => !s.IsHidden)
                                                                       .Select(s => new SectionsForRoleDTO
            {
                Id = s.Id,
                Description = s.Description,
                Name = s.Name,
                Selected = false,
            }).ToList();

            return Response<List<SectionsForRoleDTO>>.Success(sections);
        }

        public async Task<Response<RoleDTO>> UpdateAsync(RoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return Response<RoleDTO>.Failure("No se puede modificar el rol de Super Administrador.");
                }

                // Role
                PrivateBlogRole role = _mapper.Map<PrivateBlogRole>(dto);

                _context.PrivateBlogRoles.Update(role);

                // Permissions
                List<Guid> permissionIds = [];

                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                }

                // Delete old permissions
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.PrivateBlogRoleId == role.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // create new permissions
                foreach (Guid permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        PrivateBlogRoleId = role.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                // Sections
                List<Guid> sectionIds = [];

                if (!string.IsNullOrEmpty(dto.SectionIds))
                {
                    sectionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.SectionIds);
                }

                // Delete old permissions
                List<RoleSection> oldRoleSections = await _context.RoleSections.Where(rp => rp.PrivateBlogRoleId == role.Id)
                                                                               .ToListAsync();
                _context.RoleSections.RemoveRange(oldRoleSections);

                // create new sections
                foreach (Guid sectionId in sectionIds)
                {
                    RoleSection rolePermission = new RoleSection
                    {
                        PrivateBlogRoleId = role.Id,
                        SectionId = sectionId
                    };

                    await _context.RoleSections.AddAsync(rolePermission);
                }

                await _context.SaveChangesAsync();

                return Response<RoleDTO>.Success(dto, "Rol actualizado con éxito");
            }

            catch (Exception ex)
            {
                return Response<RoleDTO>.Failure(ex);
            }          
        }
    }
}
