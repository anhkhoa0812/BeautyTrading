using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Categories.Query.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<ApiResponse>
{
    public Guid CateogoryId { get; set; }
}