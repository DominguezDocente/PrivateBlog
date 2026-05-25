using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetProfile
{
    public class GetAccountProfileUseCase : IRequestHandler<GetProfileQuery, AccountProfileDTO>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountProfileUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountProfileDTO> Handle(GetProfileQuery query)
        {
            return await _accountRepository.GetProfileAsync(query.UserId);
        }
    }
}
