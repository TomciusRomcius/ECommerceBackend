using ECommerce.Domain.src.Entities;

namespace ECommerce.Domain.src.Models;

public class DetailedProductModel : ProductEntity
{
    public DetailedProductModel(
        int productId,
        string name,
        string description,
        decimal price,
        int manufacturerId,
        int categoryId,
        int stock) : base(productId, name, description, price, manufacturerId, categoryId)
    {
        Stock = stock;
    }

    public int Stock { get; set; }
}