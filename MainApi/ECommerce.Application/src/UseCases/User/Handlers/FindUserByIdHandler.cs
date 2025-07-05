using ECommerce.Application.src.UseCases.User.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.User.Handlers;

public class FindUserByidHandler : IRequestHandler<FindUserByIdQuery, UserEntity?>
{
    private readonly IUserRepository _userRepository;

    public FindUserByidHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity?> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: return result
        Result<UserEntity?> result = await _userRepository.FindByIdAsync(request.UserId.ToString());
        return result.GetValue();
    }
}