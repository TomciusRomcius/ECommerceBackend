using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using FluentValidation;

namespace ECommerce.Domain.Validators.StoreLocation
{
    public class StoreLocationEntityValidation : AbstractValidator<StoreLocationEntity>
    {
        public StoreLocationEntityValidation()
        {
            RuleFor(x => x.DisplayName).IsStoreLocationDisplayName();
            RuleFor(x => x.Address).IsStoreLocationAddress();
        }
    }

    public class UpdateStoreLocationModelValidation : AbstractValidator<UpdateStoreLocationModel>
    {
        public UpdateStoreLocationModelValidation()
        {
            RuleFor(x => x.DisplayName)
                .IsStoreLocationDisplayName()
                .When(x => x.DisplayName != null);
            RuleFor(x => x.Address)
                .IsStoreLocationAddress()
                .When(x => x.Address != null);
        }
    }

}
