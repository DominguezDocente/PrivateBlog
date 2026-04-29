using PrivateBlog.Application.Contracts.Account;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetUserHeaderInfo
{
    public sealed class GetUserHeaderInfoUseCase : IRequestHandler<GetUserHeaderInfoQuery, UserHeaderInfoDTO?>
    {
        private readonly IAccountRepository _accountRepository;

        public GetUserHeaderInfoUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<UserHeaderInfoDTO?> Handle(GetUserHeaderInfoQuery request)
        {
            return _accountRepository.GetUserHeaderInfoAsync(request.UserId, CancellationToken.None);
        }
    }
}
