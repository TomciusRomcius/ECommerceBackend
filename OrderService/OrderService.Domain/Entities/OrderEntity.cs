using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Entities;

public class OrderEntity
{
    public required Guid OrderEntityId { get; set; }
    [Required]
    public required Guid UserId { get; set; }
    [Required]
    public IEnumerable<OrderProductEntity>? OrderProducts { get; set; }
    public DateTime CreatedAt { get; set; }
}
