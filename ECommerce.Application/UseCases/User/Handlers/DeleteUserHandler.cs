using ECommerce.Application.UseCases.User.Commands;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.User.Handlers;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteAsync(request.UserId.ToString());
    }
}