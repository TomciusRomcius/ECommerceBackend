using ECommerce.Application.UseCases.User.Queries;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Repositories.User;
using MediatR;

namespace ECommerce.Application.UseCases.User
{
    public class FindUserByEmailHandler : IRequestHandler<FindUserByEmailQuery, UserEntity?>
    {
        readonly IUserRepository _userRepository;

        public FindUserByEmailHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity?> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.FindByEmailAsync(request.Email);
        }
    }
}