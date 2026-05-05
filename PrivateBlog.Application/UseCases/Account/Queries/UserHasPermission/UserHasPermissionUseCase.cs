using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.UserHasPermission
{
    public class UserHasPermissionUseCase : IRequestHandler<UserHasPermissionQuery, bool>
    {
        private readonly IAccountRepository _accountRepository;

        public UserHasPermissionUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<bool> Handle(UserHasPermissionQuery request)
        {
            return _accountRepository.UserHasPermissionAsync(request.UserId, request.PermissionCode);
        }
    }
}
