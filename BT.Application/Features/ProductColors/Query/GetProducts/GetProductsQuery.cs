using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.ProductColors.Query.GetProducts;

public class GetProductsQuery : IRequest<ApiResponse>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
}