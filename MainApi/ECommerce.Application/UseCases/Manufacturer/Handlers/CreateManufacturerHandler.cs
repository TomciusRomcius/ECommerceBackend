using ECommerce.Application.UseCases.Manufacturer.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Services.Order;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Handlers;

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