using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Commands.CreateRole
{
    public sealed class CreateRoleUseCase : IRequestHandler<CreateRoleCommand, Guid>
    {
        private readonly IRolesRepository _rolesRepository;

        public CreateRoleUseCase(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<Guid> Handle(CreateRoleCommand command)
        {
            if (await _rolesRepository.ExistsByNameAsync(command.Name))
            {
                throw new BussinesRuleException("Ya existe un rol con ese nombre.");
            }

            Role role = new Role(command.Name);

            await _rolesRepository.CreateAsync(role, command.PermissionIds);

            return role.Id;
        }
    }
}
