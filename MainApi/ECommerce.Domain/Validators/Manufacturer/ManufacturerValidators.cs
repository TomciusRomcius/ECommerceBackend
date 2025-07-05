using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using FluentValidation;

namespace ECommerce.Domain.Validators.Manufacturer
{
    public class ManufacturerEntityValidators : AbstractValidator<ManufacturerEntity>
    {
        public ManufacturerEntityValidators()
        {
            RuleFor(x => x.Name).IsManufacturerName();
        }
    }

    public class UpdateManufacturerModelValidator : AbstractValidator<UpdateManufacturerModel>
    {
        public UpdateManufacturerModelValidator()
        {
            RuleFor(x => x.Name).IsManufacturerName().When(x => x.Name != null);
        }
    }
}
