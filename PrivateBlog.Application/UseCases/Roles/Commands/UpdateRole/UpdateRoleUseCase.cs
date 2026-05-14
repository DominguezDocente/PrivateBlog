using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Application.UseCases.Roles.Commands.UpdateRole
{
    public sealed class UpdateRoleUseCase : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IRolesRepository _rolesRepository;

        public UpdateRoleUseCase(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task Handle(UpdateRoleCommand command)
        {
            Role? role = await _rolesRepository.GetByIdWithPermissionsAsync(command.Id);

            if (role is null)
            {
                throw new BussinesRuleException("El rol no existe.");
            }

            if (await _rolesRepository.ExistsByNameAsync(command.Name, excludeId: command.Id))
            {
                throw new BussinesRuleException("Ya existe otro rol con ese nombre.");
            }

            role.UpdateName(command.Name);

            await _rolesRepository.UpdateAsync(role, command.PermissionIds);
        }
    }
}
