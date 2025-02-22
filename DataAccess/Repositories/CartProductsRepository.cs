using ECommerce.DataAccess.Entities.CartProduct;
using ECommerce.DataAccess.Models.CartProduct;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;
using ECommerce.DataAccess.Utils.DictionaryExtensions;

namespace ECommerce.DataAccess.Repositories
{
    public class CartProductsRepository : ICartProductsRepository
    {
        readonly IPostgresService _postgresService;

        public CartProductsRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<CartProductEntity?> AddItemAsync(CartProductEntity cartProduct)
        {
            string query = @"
                INSERT INTO cartProducts (userId, productId, storeLocationId, quantity)
                VALUES ($1, $2, $3, $4);
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(cartProduct.UserId)),
                new QueryParameter(cartProduct.ProductId),
                new QueryParameter(cartProduct.StoreLocationId),
                new QueryParameter(cartProduct.Quantity),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);

            return cartProduct;
        }

        public async Task<List<CartProductModel>> GetUserCartProductsDetailedAsync(string userId)
        {
            string query = @"
                SELECT cartProducts.userid, cartProducts.productid, cartProducts.storelocationid, cartProducts.quantity, products.price 
                FROM cartProducts 
                INNER JOIN products
                ON products.productId = cartProducts.productId
                WHERE userId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            List<CartProductModel> result = new List<CartProductModel>();

            foreach (var row in rows)
            {
                result.Add(new CartProductModel(
                    row.GetColumn<Guid>("userid").ToString(),
                    row.GetColumn<int>("productid"),
                    row.GetColumn<int>("storelocationid"),
                    row.GetColumn<int>("quantity"),
                    (double)row.GetColumn<decimal>("price") // TODO: decimal type
                ));
            }

            return result;
        }

        public async Task<List<CartProductEntity>> GetUserCartProductsAsync(string userId)
        {
            string query = @"
                SELECT cartProducts.userid, cartProducts.productid, cartProducts.storelocationid, cartProducts.quantity 
                FROM cartProducts 
                WHERE userId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            List<CartProductEntity> result = new List<CartProductEntity>();

            foreach (var row in rows)
            {
                result.Add(new CartProductEntity(
                    row.GetColumn<Guid>("userid").ToString(),
                    row.GetColumn<int>("productid"),
                    row.GetColumn<int>("storelocationid"),
                    row.GetColumn<int>("quantity")
                ));
            }

            return result;
        }

        public async Task RemoveAllCartItemsAsync(Guid userId)
        {
            string query = @"
                DELETE FROM cartProducts WHERE userId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(userId),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task RemoveItemAsync(string userId, int productId)
        {
            string query = @"
                DELETE FROM cartProducts WHERE userId = $1 AND productId = $2;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
                new QueryParameter(productId),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task<CartProductEntity?> UpdateItemAsync(CartProductEntity cartProduct)
        {
            string query = @"
                UPDATE cartProducts
                SET
                    quantity = $1
                WHERE userId = $2 AND productId = $3 
            ";

            QueryParameter[] parameters = [
                new QueryParameter(cartProduct.Quantity),
                new QueryParameter(new Guid(cartProduct.UserId)),
                new QueryParameter(cartProduct.ProductId)
            ];


            await _postgresService.ExecuteScalarAsync(query, parameters);

            return cartProduct;
        }
    }
}