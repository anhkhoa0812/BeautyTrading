using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Orders.Command.CreateOrder;

public class CreateOrderCommand : IRequest<ApiResponse>
{
    public List<CreateOrderItemDto> Items { get; set; }
}