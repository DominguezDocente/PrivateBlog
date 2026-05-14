using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRoleById
{
    public sealed class GetRoleByIdUseCase : IRequestHandler<GetRoleByIdQuery, RoleDetailDTO>
    {
        private readonly IRolesRepository _rolesRepository;

        public GetRoleByIdUseCase(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<RoleDetailDTO> Handle(GetRoleByIdQuery query)
        {
            Role? role = await _rolesRepository.GetByIdWithPermissionsAsync(query.Id);

            if (role is null)
            {
                throw new BussinesRuleException("El rol no existe.");
            }

            return new RoleDetailDTO
            {
                Id = role.Id,
                Name = role.Name,
                PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId)
                                                    .ToList(),
            };
        }
    }
}
