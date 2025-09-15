namespace BT.Domain.Models.OrderItems;

public class CreateOrderItemsResponse
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid ProductVariantId { get; set; }
}