using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.OrderItems;

namespace BT.Domain.Models.Orders;

public class CreateOrderResponse
{
    public Guid Id { get; set; }
    public EOrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CreateOrderItemsResponse> OrderItems { get; set; }
}