using ECommerce.DataAccess.Models.CartProduct;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.DataAccess.Repositories
{
    public class CartProductsRepository : ICartProductsRepository
    {
        readonly IPostgresService _postgresService;

        public CartProductsRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<CartProductModel?> AddItemAsync(CartProductModel cartProductModel)
        {
            string query = @"
                INSERT INTO cartProducts (userId, productId, quantity)
                VALUES ($1, $2, $3);
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(cartProductModel.UserId)),
                new QueryParameter(cartProductModel.ProductId),
                new QueryParameter(cartProductModel.Quantity),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);

            return cartProductModel;
        }

        public async Task<List<CartProductModel>> GetUserCartProductsAsync(string userId)
        {
            string query = @"
                SELECT * from cartProducts WHERE userId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            List<CartProductModel> result = new List<CartProductModel>();

            foreach (var row in rows)
            {
                // TODO: null safety
                result.Add(new CartProductModel(
                    row["userid"].ToString(),
                    Convert.ToInt32(row["productid"]),
                    Convert.ToInt32(row["quantity"])
                ));
            }

            return result;
        }

        public async Task RemoveAllCartItemsAsync(string userId)
        {
            string query = @"
                DELETE FROM cartProducts WHERE userId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
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

        public async Task<CartProductModel?> UpdateItemAsync(CartProductModel cartProduct)
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