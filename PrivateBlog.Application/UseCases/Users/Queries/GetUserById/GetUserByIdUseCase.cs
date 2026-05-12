using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetUserById
{
    public sealed class GetUserByIdUseCase : IRequestHandler<GetUserByIdQuery, UserDetailDTO>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUserByIdUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<UserDetailDTO> Handle(GetUserByIdQuery query)
        {
            User? user = await _usersRepository.GetByIdAsync(query.Id);

            if (user is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            return new UserDetailDTO
            {
                Id = user.Id,
                FirstName = user.FisrtName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
            };
        }
    }
}
