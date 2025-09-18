using BT.Domain.Enums;

namespace BT.Domain.Models.Orders;

public class GetOrderResponse
{
    public Guid Id { get; set; }
    
    public EOrderStatus Status { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public string? Address { get; set; }
    
    public string? Country { get; set; }
    
    public string? TaxCode { get; set; }
}