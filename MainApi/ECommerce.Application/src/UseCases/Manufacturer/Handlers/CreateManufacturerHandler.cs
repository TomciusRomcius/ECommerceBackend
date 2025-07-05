using ECommerce.Application.src.UseCases.Manufacturer.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.src.Repositories;
using ECommerce.Domain.src.Services.Order;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Manufacturer.Handlers;

public class CreateManufacturerHandler : IRequestHandler<CreateManufacturerCommand, Result<int>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IObjectValidator _objectValidator;

    public CreateManufacturerHandler(IManufacturerRepository manufacturerRepository, IObjectValidator objectValidator)
    {
        _manufacturerRepository = manufacturerRepository;
        _objectValidator = objectValidator;
    }

    public async Task<Result<int>> Handle(CreateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        return await _manufacturerRepository.CreateAsync(request.Name);
    }
}