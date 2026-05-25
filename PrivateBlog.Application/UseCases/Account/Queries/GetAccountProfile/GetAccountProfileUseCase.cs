using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile
{
    public class GetAccountProfileUseCase : IRequestHandler<GetAccountProfileQuery, AccountProfileDTO>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountProfileUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountProfileDTO> Handle(GetAccountProfileQuery request)
        {
            return await _accountRepository.GetProfileAsync(request.UserId);
        }
    }
}
