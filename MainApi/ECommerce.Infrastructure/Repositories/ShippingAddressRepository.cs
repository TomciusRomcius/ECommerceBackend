using System.Data;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Repositories;

public class ShippingAddressRepository : IShippingAddressRepository
{
    private readonly IPostgresService _postgresService;

    public ShippingAddressRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task AddAddressAsync(ShippingAddressEntity addressModel)
    {
        var query = @"
                INSERT INTO shippingAddresses
                (userId, recipientName, streetAddress, apartmentUnit, country, city, state, postalCode, mobileNumber)
                VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)
                RETURNING shippingAddressId;
            ";

        // TODO: add apartment
        QueryParameter[] parameters =
        [
            new(new Guid(addressModel.UserId)),
            new(addressModel.RecipientName),
            new(addressModel.StreetAddress),
            new(addressModel.ApartmentUnit),
            new(addressModel.Country),
            new(addressModel.City),
            new(addressModel.State),
            new(addressModel.PostalCode),
            new(addressModel.MobileNumber)
        ];

        object? id = await _postgresService.ExecuteScalarAsync(query, parameters);

        if (id is not null)
            addressModel.ShippingAddressId = Convert.ToInt64(id);

        else throw new DataException("shippingAddressId is null!");
    }

    public async Task DeleteAddressAsync(string userId)
    {
        var query = @"
                DELETE FROM shippingAddress WHERE userId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(userId))
        ];

        await _postgresService.ExecuteAsync(query, parameters);
    }

    public async Task<List<ShippingAddressEntity>> GetAddresses(string userId)
    {
        var query = @"
                SELECT * from shippingAddresses WHERE userId = $1;
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(userId))
        ];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        var result = new List<ShippingAddressEntity>();

        foreach (Dictionary<string, object> row in rows)
        {
            var address = new ShippingAddressEntity
            {
                ShippingAddressId = row.GetColumn<long>("shippingaddressid"),
                UserId = row.GetColumn<Guid>("userid").ToString(),
                RecipientName = row.GetColumn<string>("recipientname"),
                StreetAddress = row.GetColumn<string>("streetaddress"),
                ApartmentUnit = row.GetColumn<string?>("apartmentunit"),
                City = row.GetColumn<string>("city"),
                State = row.GetColumn<string>("state"),
                PostalCode = row.GetColumn<string>("postalcode"),
                Country = row.GetColumn<string>("country"),
                MobileNumber = row.GetColumn<string>("mobilenumber")
            };

            result.Add(address);
        }

        return result;
    }

    public async Task UpdateAddressAsync(UpdateShippingAddressModel updateAddressModel)
    {
        var query = @"
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

        QueryParameter[] parameters =
        [
            new(updateAddressModel.RecipientName),
            new(updateAddressModel.StreetAddress),
            new(updateAddressModel.ApartmentUnit),
            new(updateAddressModel.Country),
            new(updateAddressModel.City),
            new(updateAddressModel.State),
            new(updateAddressModel.PostalCode),
            new(updateAddressModel.MobileNumber),
            new(new Guid(updateAddressModel.UserId)),
            new(updateAddressModel.ShippingAddressId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }
}