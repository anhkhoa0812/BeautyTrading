using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Products.Command.UpdateProduct;

public class UpdateProductCommand : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
    public IFormFile? BannerImage { get; set; }
    public string? VideoUrl { get; set; }
    public IFormFile? Image { get; set; }
}