using ECommerce.DataAccess.Models.Address;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.DataAccess.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        readonly IPostgresService _postgresService;

        public AddressRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task AddAddressAsync(AddressModel addressModel)
        {
            string query = @"
                INSERT INTO addresses
                (userId, isShipping, recipientName, streetAddress, apartmentUnit, country, city, state, postalCode, mobileNumber)
                VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10);
            ";

            // TODO: add apartment
            QueryParameter[] parameters = [
                new QueryParameter(new Guid(addressModel.UserId)),
                new QueryParameter(addressModel.IsShipping),
                new QueryParameter(addressModel.RecipientName),
                new QueryParameter(addressModel.StreetAddress),
                new QueryParameter(addressModel.ApartmentUnit),
                new QueryParameter(addressModel.Country),
                new QueryParameter(addressModel.City),
                new QueryParameter(addressModel.State),
                new QueryParameter(addressModel.PostalCode),
                new QueryParameter(addressModel.MobileNumber),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task DeleteAddressAsync(string userId, bool isShipping)
        {
            string query = @"
                DELETE FROM addresses WHERE userId = $1 AND isShipping = $2;
            ";


            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
                new QueryParameter(isShipping),
            ];

            await _postgresService.ExecuteAsync(query, parameters);
        }

        public async Task<List<AddressModel>> GetAddresses(string userId)
        {
            string query = @"
                SELECT * from addresses WHERE userId = $1;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userId)),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);

            if (rows.Count > 2)
            {
                // Handle
            }

            List<AddressModel> result = new List<AddressModel>();

            foreach (var row in rows)
            {
                // TODO: null safety
                var address = new AddressModel
                {
                    UserId = userId,
                    IsShipping = (bool)row["isshipping"],
                    RecipientName = row["recipientname"].ToString(),
                    StreetAddress = row["streetaddress"].ToString(),
                    ApartmentUnit = row["apartmentunit"].ToString(),
                    City = row["city"].ToString(),
                    State = row["state"].ToString(),
                    PostalCode = row["postalcode"].ToString(),
                    Country = row["country"].ToString(),
                    MobileNumber = row["mobilenumber"].ToString(),
                };

                result.Add(address);
            }

            return result;
        }

        public async Task UpdateAddressAsync(UpdateAddressModel updateAddressModel)
        {
            string query = @"
                UPDATE addresses
                SET
                recipientName = COALESCE($1, recipientName),
                streetAddress = COALESCE($2, streetAddress),
                apartmentUnit = COALESCE($3, apartmentUnit),
                country = COALESCE($4, country),
                city = COALESCE($5, city),
                state = COALESCE($6, state),
                postalCode = COALESCE($7, postalCode),
                mobileNumber = COALESCE($8, mobileNumber)
                WHERE userId = $9 AND isShipping = $10;
            ";

            // TODO: add apartment
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
                new QueryParameter(updateAddressModel.IsShipping)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}