using ECommerce.Application.UseCases.User.Commands;
using ECommerce.Application.UseCases.User.Queries;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Repositories.User;
using MediatR;

namespace ECommerce.Application.UseCases.User
{
    public class FindUserByidHandler : IRequestHandler<FindUserByIdQuery, UserEntity?>
    {
        readonly IUserRepository _userRepository;

        public FindUserByidHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity?> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.FindByIdAsync(request.UserId.ToString());
        }
    }
}