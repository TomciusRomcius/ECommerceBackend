using ECommerce.Application.UseCases.Manufacturer.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Services.Order;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Handlers;

public class CreateManufacturerHandler : IRequestHandler<CreateManufacturerCommand, Result<ManufacturerEntity>>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IObjectValidator _objectValidator;

    public CreateManufacturerHandler(IManufacturerRepository manufacturerRepository, IObjectValidator objectValidator)
    {
        _manufacturerRepository = manufacturerRepository;
        _objectValidator = objectValidator;
    }

    public async Task<Result<ManufacturerEntity>> Handle(CreateManufacturerCommand request,
        CancellationToken cancellationToken)
    {
        IEnumerable<ResultError> errors = _objectValidator.Validate(new ManufacturerEntity(1, request.Name));
        if (errors.Any()) return new Result<ManufacturerEntity>(null, errors);

        ManufacturerEntity? manufacturer = await _manufacturerRepository.CreateAsync(request.Name);
        return new Result<ManufacturerEntity>(manufacturer);
    }
}