using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Orders.Command.CreateOrder;

public class CreateOrderCommand : IRequest<ApiResponse>
{
    public string Address { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public List<CreateOrderItemDto> Items { get; set; }
}