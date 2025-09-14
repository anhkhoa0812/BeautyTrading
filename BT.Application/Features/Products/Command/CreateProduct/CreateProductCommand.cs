using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile MainImage { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsHasVariants { get; set; }
    public IFormFile BannerImage { get; set; }
    public string? VideoUrl { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? Stock { get; set; }
    public List<CreateProductVariantRequest>? Variants { get; set; }
    public List<CreateProductImageRequest>? Images { get; set; }
}

public class CreateProductVariantRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public int Stock { get; set; }
}
public class CreateProductImageRequest
{
    public IFormFile Image { get; set; }
}