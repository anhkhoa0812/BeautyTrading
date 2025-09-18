using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.News.Command.CreateNewsImage;

public class CreateNewImageCommand : IRequest<ApiResponse>
{
    public IFormFile Image { get; set; }
}