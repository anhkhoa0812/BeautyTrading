using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.News.Command.UpdateNews;

public class UpdateNewsCommand : IRequest<ApiResponse>
{
    public Guid NewsId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
    public bool? IsActive { get; set; }
}
public class UpdateNewsRequest
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
    public bool? IsActive { get; set; }
}