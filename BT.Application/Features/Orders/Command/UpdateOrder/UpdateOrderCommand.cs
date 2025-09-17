using BT.Domain.Enums;
using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Orders.Command.UpdateOrder;

public class UpdateOrderCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    
    public EOrderStatus Status { get; set; }
}

public class UpdateOrderRequest
{
    public EOrderStatus Status { get; set; }
}