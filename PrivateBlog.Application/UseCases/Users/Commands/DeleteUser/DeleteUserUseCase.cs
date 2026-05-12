using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Commands.DeleteUser
{
    public sealed class DeleteUserUseCase : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUsersRepository _usersRepository;

        public DeleteUserUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task Handle(DeleteUserCommand command)
        {
            User? user = await _usersRepository.GetByIdAsync(command.Id);

            if (user is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            await _usersRepository.DeleteAsync(command.Id);
        }
    }
}
