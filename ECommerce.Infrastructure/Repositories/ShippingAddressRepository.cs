using System.Data;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.ShippingAddress;
using ECommerce.Domain.Models.ShippingAddress;
using ECommerce.Domain.Repositories.ShippingAddress;

namespace ECommerce.Infrastructure.Repositories.ShippingAddress
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        readonly IPostgresService _postgresService;

        public ShippingAddressRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task AddAddressAsync(ShippingAddressEntity addressModel)
        {
            string query = @"
                INSERT INTO shippingAddresses
                (userId, recipientName, streetAddress, apartmentUnit, country, city, state, postalCode, mobileNumber)
                VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)
                RETURNING shippingAddressId;
            ";

            // TODO: add apartment
            QueryParameter[] parameters = [
                new QueryParameter(new Guid(addressModel.UserId)),
                new QueryParameter(addressModel.RecipientName),
                new QueryParameter(addressModel.StreetAddress),
                new QueryParameter(addressModel.ApartmentUnit),
                new QueryParameter(addressModel.Country),
                new QueryParameter(addressModel.City),
                new QueryParameter(addressModel.State),
                new QueryParameter(addressModel.PostalCode),
                new QueryParameter(addressModel.MobileNumber),
            ];

            object? id = await _postgresService.ExecuteScalarAsync(query, parameters);

            if (id is not null)
            {
                addressModel.ShippingAddressId = Convert.ToInt64(id);
            }

            else throw new DataException("shippingAddressId is null!");
        }

        public async Task DeleteAddressAsync(string userId, bool isShipping)
        {
            string query = @"
                DELETE FROM shippingAddress WHERE userId = $1 AND isShipping = $2;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
                new QueryParameter(isShipping),
            ];

            await _postgresService.ExecuteAsync(query, parameters);
        }

        public async Task<List<ShippingAddressEntity>> GetAddresses(string userId)
        {
            string query = @"
                SELECT * from shippingAddresses WHERE userId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            List<ShippingAddressEntity> result = new List<ShippingAddressEntity>();

            foreach (var row in rows)
            {
                var address = new ShippingAddressEntity
                {
                    ShippingAddressId = row.GetColumn<Int64>("shippingaddressid"),
                    UserId = row.GetColumn<Guid>("userid").ToString(),
                    RecipientName = row.GetColumn<string>("recipientname"),
                    StreetAddress = row.GetColumn<string>("streetaddress"),
                    ApartmentUnit = row.GetColumn<string?>("apartmentunit"),
                    City = row.GetColumn<string>("city"),
                    State = row.GetColumn<string>("state"),
                    PostalCode = row.GetColumn<string>("postalcode"),
                    Country = row.GetColumn<string>("country"),
                    MobileNumber = row.GetColumn<string>("mobilenumber"),
                };

                result.Add(address);
            }

            return result;
        }

        public async Task UpdateAddressAsync(UpdateShippingAddressModel updateAddressModel)
        {
            string query = @"
                UPDATE shippingAddresses
                SET
                recipientName = COALESCE($1, recipientName),
                streetAddress = COALESCE($2, streetAddress),
                apartmentUnit = COALESCE($3, apartmentUnit),
                country = COALESCE($4, country),
                city = COALESCE($5, city),
                state = COALESCE($6, state),
                postalCode = COALESCE($7, postalCode),
                mobileNumber = COALESCE($8, mobileNumber)
                WHERE userId = $9 AND addressId = $10;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(updateAddressModel.RecipientName),
                new QueryParameter(updateAddressModel.StreetAddress),
                new QueryParameter(updateAddressModel.ApartmentUnit),
                new QueryParameter(updateAddressModel.Country),
                new QueryParameter(updateAddressModel.City),
                new QueryParameter(updateAddressModel.State),
                new QueryParameter(updateAddressModel.PostalCode),
                new QueryParameter(updateAddressModel.MobileNumber),
                new QueryParameter(new Guid(updateAddressModel.UserId)),
                new QueryParameter(updateAddressModel.ShippingAddressId)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}