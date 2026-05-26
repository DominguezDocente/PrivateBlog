using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetBlogById
{
    public class GetAccessibleBlogByIdUseCase : IRequestHandler<GetAccessibleBlogByIdQuery, AccessibleBlogDetailDTO>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccessibleBlogByIdUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<AccessibleBlogDetailDTO> Handle(GetAccessibleBlogByIdQuery request)
        {
            return _accountRepository.GetAccessibleBlogByIdAsync(request.UserId, request.BlogId);
        }
    }
}
