using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Categories.Command.CreateCategory;

public class CreateCategoryCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}