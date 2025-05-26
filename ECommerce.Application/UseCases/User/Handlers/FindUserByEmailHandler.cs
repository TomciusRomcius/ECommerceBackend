using ECommerce.Application.UseCases.User.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.UseCases.User.Handlers;

public class FindUserByEmailHandler : IRequestHandler<FindUserByEmailQuery, UserEntity?>
{
    private readonly IUserRepository _userRepository;

    public FindUserByEmailHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity?> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.FindByEmailAsync(request.Email);
    }
}