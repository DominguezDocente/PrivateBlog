using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo
{
    public class GetAccountUserInfoUseCase : IRequestHandler<GetAccountUserInfoQuery, UserAccountInfoDTO>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountUserInfoUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<UserAccountInfoDTO> Handle(GetAccountUserInfoQuery request)
        {
            return _accountRepository.GetUserInfoAsync(request.UserId);
        }
    }
}
