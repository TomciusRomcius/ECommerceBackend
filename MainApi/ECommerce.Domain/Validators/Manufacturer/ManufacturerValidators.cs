using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
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
