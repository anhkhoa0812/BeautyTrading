using BT.Domain.Enums;

namespace BT.Domain.Models.Orders;

public class GetOrderResponse
{
    public Guid Id { get; set; }
    
    public EOrderStatus Status { get; set; }
    
    public decimal TotalPrice { get; set; }
}