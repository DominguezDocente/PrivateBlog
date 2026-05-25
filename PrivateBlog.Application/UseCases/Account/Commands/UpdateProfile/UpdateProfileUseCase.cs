using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Commands.UpdateProfile
{
    public class UpdateProfileUseCase : IRequestHandler<UpdateProfileCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public UpdateProfileUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Handle(UpdateProfileCommand request)
        {
            await _accountRepository.UpdateProfileAsync(
                request.UserId,
                request.FirstName,
                request.LastName,
                request.PhoneNumber);
        }
    }
}
