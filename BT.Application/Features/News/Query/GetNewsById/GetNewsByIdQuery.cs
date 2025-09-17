using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.News.Query.GetNewsById;

public class GetNewsByIdQuery : IRequest<ApiResponse>
{
    public Guid NewsId { get; set; }
}