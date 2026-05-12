using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Domain.Entities.Account;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetRoleOptions
{
    public sealed class GetRoleOptionsUseCase : IRequestHandler<GetRoleOptionsQuery, IReadOnlyList<RoleOptionDTO>>
    {
        private readonly IUsersRepository _usersRepository;

        public GetRoleOptionsUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IReadOnlyList<RoleOptionDTO>> Handle(GetRoleOptionsQuery query)
        {
            List<Role> roles = await _usersRepository.GetRolesAsync();

            return roles.Select(r => new RoleOptionDTO
            {
                Id = r.Id,
                Name = r.Name,
            }).ToList();
        }
    }
}
