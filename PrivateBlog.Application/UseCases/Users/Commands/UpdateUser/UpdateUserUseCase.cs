using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed class UpdateUserUseCase : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUsersRepository _usersRepository;

        public UpdateUserUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task Handle(UpdateUserCommand command)
        {
            User? existing = await _usersRepository.GetByIdAsync(command.Id);

            if (existing is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            User updated = User.Reconstitute(
                command.Id,
                command.FirstName,
                command.LastName,
                command.Email,
                command.Email,
                existing.EmailConfirmed,
                command.PhoneNumber,
                command.RoleId);

            await _usersRepository.UpdateAsync(updated);
        }
    }
}
