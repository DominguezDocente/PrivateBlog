using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Users.Commands.CreateUser
{
    public sealed class CreateUserUseCase : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUsersRepository _usersRepository;

        public CreateUserUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<string> Handle(CreateUserCommand command)
        {
            User user = User.Reconstitute(
                                       id: Guid.CreateVersion7().ToString(),
                                       firstName: command.FirstName,
                                       lastName: command.LastName,
                                       userName: command.Email,
                                       email: command.Email,
                                       emailConfirmed: true,
                                       phoneNumber: command.PhoneNumber,
                                       roleId: command.RoleId);

            await _usersRepository.CreateAsync(user, command.Password);

            return user.Id;
        }
    }
}
