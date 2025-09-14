using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Products.Query.GetProductById;

public class GetProductByIdQuery : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
}