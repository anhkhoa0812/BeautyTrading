namespace BT.Domain.Models.OrderItems;

public class GetOrderItemsResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public decimal Price { get; set; }
    
    public int Quantity { get; set; }
    
    public string ProductColor { get; set; }
}