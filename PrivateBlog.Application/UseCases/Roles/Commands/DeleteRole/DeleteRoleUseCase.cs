using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Roles.Commands.DeleteRole
{
    public sealed class DeleteRoleUseCase : IRequestHandler<DeleteRoleCommand>
    {
        private readonly IRolesRepository _rolesRepository;

        public DeleteRoleUseCase(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task Handle(DeleteRoleCommand command)
        {
            Role? role = await _rolesRepository.GetByIdWithPermissionsAsync(command.Id);

            if (role is null)
            {
                throw new BussinesRuleException("El rol no existe.");
            }

            if (await _rolesRepository.HasUsersAsync(command.Id))
            {
                throw new BussinesRuleException("No se puede eliminar el rol porque tiene usuarios asociados.");
            }

            await _rolesRepository.DeleteAsync(command.Id);
        }
    }
}
