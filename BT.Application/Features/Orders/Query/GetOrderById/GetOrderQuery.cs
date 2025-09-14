using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Orders.Query.GetOrderById;

public class GetOrderQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}