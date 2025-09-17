using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.News.Query.GetNews;

public class GetNewsQuery : IRequest<ApiResponse>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
}