using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogsBySection
{
    public class GetAccessibleBlogsBySectionUseCase : IRequestHandler<GetAccessibleBlogsBySectionQuery, AccessibleSectionBlogsDTO>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccessibleBlogsBySectionUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<AccessibleSectionBlogsDTO> Handle(GetAccessibleBlogsBySectionQuery request)
        {
            return _accountRepository.GetAccessibleBlogsBySectionAsync(request.UserId, request.SectionId);
        }
    }
}
