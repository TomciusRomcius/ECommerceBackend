using ECommerce.Application.src.UseCases.User.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.User.Handlers;

public class FindUserByEmailHandler : IRequestHandler<FindUserByEmailQuery, UserEntity?>
{
    private readonly IUserRepository _userRepository;

    public FindUserByEmailHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity?> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
    {
        // TODO: return result
        Result<UserEntity?> result = await _userRepository.FindByEmailAsync(request.Email);
        return result.GetValue();
    }
}