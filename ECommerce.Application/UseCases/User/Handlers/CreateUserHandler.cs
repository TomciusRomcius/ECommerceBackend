using ECommerce.Application.UseCases.User.Commands;
using ECommerce.Domain.Repositories.User;
using MediatR;

namespace ECommerce.Application.UseCases.User
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand>
    {
        readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.CreateAsync(request.User);
        }
    }
}