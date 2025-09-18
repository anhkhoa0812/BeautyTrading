using BT.Application.Features.News.Command.CreateNewsImage;
using BT.Domain.Constants;
using BT.Domain.Models.Common;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

public class ImageController : BaseController<ImageController>
{
    private readonly IMediator _mediator;
    
    public ImageController(ILogger logger, IMediator mediator) : base(logger)
    {
        _mediator = mediator;
    }
    
    [HttpPost(ApiEndPointConstant.Image.CreateImage)]
    [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateImage([FromForm] CreateNewImageCommand command)
    {
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateImage), apiResponse);
    }
}