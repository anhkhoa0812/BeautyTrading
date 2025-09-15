using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.ProductVariants.Command.UpdateProductVariant;

public class UpdateProductVariantCommand : IRequest<ApiResponse>
{
    public Guid ProductVariantId { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? Stock { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateProductVariantRequest
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? Stock { get; set; }
    public bool? IsActive { get; set; }
}