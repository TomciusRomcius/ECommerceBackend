using System.ComponentModel.DataAnnotations;
using ECommerce.Domain.Entities.ProductStoreLocation;
using ECommerce.Domain.Models.CartProduct;

namespace ECommerce.Domain.Services
{
    public interface IOrderValidator
    {
        public void Validate(List<CartProductModel> cartProducts, List<ProductStoreLocationEntity> productStoreLocations);
    }

    public class OrderValidator : IOrderValidator
    {

        public void Validate(List<CartProductModel> cartProducts, List<ProductStoreLocationEntity> productStoreLocations)
        {
            if (cartProducts.Count() == 0)
            {
                throw new ValidationException("Cannot proceed with creating an order because the user cart is empty");
            }

            foreach (var item in cartProducts)
            {
                ProductStoreLocationEntity? selected = productStoreLocations.Find((psl) =>
                    psl.ProductId == item.ProductId && psl.StoreLocationId == item.StoreLocationId
                );

                if (selected is null)
                {
                    throw new ValidationException("Cannot proceed with creating an order the product does not exist");
                }

                if (item.Quantity > selected.Stock)
                {
                    throw new ValidationException("Cannot proceed with creating desired product quantity is higher than its stock");
                }
            }
        }
    }
}
