using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.src.Entities;

public class ShippingAddressEntity
{
    [Key]
    public long ShippingAddressId { get; set; }

    [ForeignKey(nameof(User))]
    public required string UserId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Recipient name cannot be empty!")]
    public required string RecipientName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Street address cannot be empty!")]
    public required string StreetAddress { get; set; }

    // TODO: Validation when apartment unit is provided
    public required string? ApartmentUnit { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "City cannot be empty!")]
    public required string City { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "State cannot be empty!")]
    public required string State { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Postal code cannot be empty!")]
    public required string PostalCode { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Country cannot be empty!")]
    public required string Country { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile number cannot be empty!")]
    public required string MobileNumber { get; set; }

    public IdentityUser? User { get; set; }
}