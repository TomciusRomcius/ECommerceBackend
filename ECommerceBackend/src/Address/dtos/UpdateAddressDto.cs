using System.ComponentModel.DataAnnotations;

namespace ECommerce.Address
{
    public class UpdateAddressDto
    {
        // Billing - 0, Shipping - 1

        public bool IsShipping { get; set; }
        public string? RecipientName { get; set; } = null;
        public string? StreetAddress { get; set; } = null;
        public string? ApartmentUnit { get; set; } = null;
        public string? City { get; set; } = null;
        public string? State { get; set; } = null;
        public string? PostalCode { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? MobileNumber { get; set; } = null;
    }
}