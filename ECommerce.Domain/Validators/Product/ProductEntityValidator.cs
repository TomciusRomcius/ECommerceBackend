using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.Domain.Validators.Product
{
    public class ProductEntityValidator : AbstractValidator<ProductEntity>
    {
        public ProductEntityValidator()
        {
            RuleFor(x => x.Name).IsProductName();
            RuleFor(x => x.Description).IsProductDescription();
            RuleFor(x => x.Price).IsProductPrice();
        }
    }
}
