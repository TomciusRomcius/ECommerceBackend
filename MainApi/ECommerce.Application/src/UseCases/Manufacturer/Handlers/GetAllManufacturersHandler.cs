using ECommerce.Application.src.UseCases.Manufacturer.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using MediatR;

namespace ECommerce.Application.src.UseCases.Manufacturer.Handlers;

public class GetAllManufacturersHandler : IRequestHandler<GetAllManufacturersQuery, List<ManufacturerEntity>>
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public GetAllManufacturersHandler(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<List<ManufacturerEntity>> Handle(GetAllManufacturersQuery request,
        CancellationToken cancellationToken)
    {
        return await _manufacturerRepository.GetAll();
    }
}