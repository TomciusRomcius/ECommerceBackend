using ECommerce.Application.UseCases.User.Commands;
using ECommerce.Domain.Repositories.User;
using MediatR;

namespace ECommerce.Application.UseCases.User
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateAsync(request.Updator);
        }
    }
}