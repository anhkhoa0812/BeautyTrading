namespace BT.Application.Features.Orders.Command.CreateOrder;

public class CreateOrderItemDto
{
    public Guid ProductVariantId { get; set; }
    
    public int Quantity { get; set; }
}