using ECommerce.Application.src.UseCases.User.Commands;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.User.Handlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateAsync(request.Updator);
    }
}