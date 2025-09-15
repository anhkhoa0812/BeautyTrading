using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.ProductVariants.Command.CreateProductVariant;

public class CreateProductVariantCommand : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public int Stock { get; set; }
}
public class CreateProductVariantRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public int Stock { get; set; }
}