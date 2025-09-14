using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.ProductColors.Command.CreateProductColor;

public class CreateProductColorCommand : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
    public string ColorName { get; set; }
    public IFormFile Image { get; set; }
}

public class CreateProductColorRequest
{
    public string ColorName { get; set; }
    public IFormFile Image { get; set; }
}