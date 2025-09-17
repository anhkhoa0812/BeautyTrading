using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.News.Command.CreateNews;

public class CreateNewsCommand : IRequest<ApiResponse>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public IFormFile Image { get; set; }
}