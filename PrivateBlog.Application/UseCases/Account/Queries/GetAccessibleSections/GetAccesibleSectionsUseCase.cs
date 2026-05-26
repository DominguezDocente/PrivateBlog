using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleSections
{
    public class GetAccessibleSectionsUseCase : IRequestHandler<GetAccessibleSectionsQuery, IReadOnlyList<AccessibleSectionItemDTO>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccessibleSectionsUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<IReadOnlyList<AccessibleSectionItemDTO>> Handle(GetAccessibleSectionsQuery request)
        {
            return _accountRepository.GetAccessibleSectionsAsync(request.UserId);
        }
    }
}