using ECommerce.Application.UseCases.User.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.User.Handlers;

public class FindUserByidHandler : IRequestHandler<FindUserByIdQuery, UserEntity?>
{
    private readonly IUserRepository _userRepository;

    public FindUserByidHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity?> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.FindByIdAsync(request.UserId.ToString());
    }
}