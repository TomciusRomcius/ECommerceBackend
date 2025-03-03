using System.ComponentModel.DataAnnotations;
using ECommerce.Application.UseCases.Manufacturer.Commands;
using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Repositories.Manufacturer;
using ECommerce.Domain.Services;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Manufacturer.Handlers
{
    public class CreateManufacturerHandler : IRequestHandler<CreateManufacturerCommand, Result<ManufacturerEntity>>
    {
        readonly IManufacturerRepository _manufacturerRepository;
        readonly IObjectValidator _objectValidator;

        public CreateManufacturerHandler(IManufacturerRepository manufacturerRepository, IObjectValidator objectValidator)
        {
            _manufacturerRepository = manufacturerRepository;
            _objectValidator = objectValidator;
        }

        public async Task<Result<ManufacturerEntity>> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<ResultError> errors = _objectValidator.Validate(new ManufacturerEntity(1, request.Name));
            if (errors.Any())
            {
                return new Result<ManufacturerEntity>(null, errors);
            }

            var manufacturer = await _manufacturerRepository.CreateAsync(request.Name);
            return new Result<ManufacturerEntity>(manufacturer);
        }
    }
}