using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;

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
                Guid.CreateVersion7().ToString(),
                command.RoleId,
                command.FirstName,
                command.LastName,
                command.Email,
                command.Email,
                emailConfirmed: true,
                command.PhoneNumber);

            await _usersRepository.CreateAsync(user, command.Password);

            return user.Id;
        }
    }
}
