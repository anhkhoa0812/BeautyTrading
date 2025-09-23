using BT.Application.Common.Utils;
using BT.Application.Common.Validators;
using BT.Application.Features.News.Command.CreateNews;
using BT.Application.Features.News.Command.UpdateNews;
using BT.Application.Features.News.Query.GetNews;
using BT.Application.Features.News.Query.GetNewsById;
using BT.Domain.Constants;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.News;
using BT.Infrastructure.Paginate.Interface;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.News.NewsEndpoint)]
public class NewsController : BaseController<NewsController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<CreateNewsCommand> _createNewsValidatorUtil;
    private readonly ValidationUtil<UpdateNewsCommand> _updateNewsValidatorUtil;
    
    
    
    public NewsController(ILogger logger, IMediator mediator,
        ValidationUtil<CreateNewsCommand> createNewsValidatorUtil,
        ValidationUtil<UpdateNewsCommand> updateNewsValidatorUtil) : base(logger)
    {
        _mediator = mediator;
        _createNewsValidatorUtil = createNewsValidatorUtil;
        _updateNewsValidatorUtil = updateNewsValidatorUtil;
    }
    
    [CustomAuthorize(ERole.Admin)]
    [HttpPost(ApiEndPointConstant.News.NewsEndpoint)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateNews([FromForm] CreateNewsCommand command)
    {
        var (isValid, response) = await _createNewsValidatorUtil.ValidateAsync(command);
        if (!isValid) return BadRequest(response);
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateNews), apiResponse);
    }

    [CustomAuthorize(ERole.Admin)]
    [HttpPatch(ApiEndPointConstant.News.NewsWithId)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateNews([FromRoute] Guid id, [FromForm] UpdateNewsRequest request)
    {
        var command = new UpdateNewsCommand()
        {
            NewsId = id,
            Title = request.Title,
            Content = request.Content,
            Image = request.Image,
            IsActive = request.IsActive
        };
        var (isValid, response) = await _updateNewsValidatorUtil.ValidateAsync(command);
        if (!isValid) return BadRequest(response);
        
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    
    [HttpGet(ApiEndPointConstant.News.NewsEndpoint)]
    [ProducesResponseType(typeof(ApiResponse<IPaginate<GetNewsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetNews([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
    {
        var query = new GetNewsQuery()
        {
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    
    [HttpGet(ApiEndPointConstant.News.NewsWithId)]
    [ProducesResponseType(typeof(ApiResponse<GetNewsByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetNewsById([FromRoute] Guid id)
    {
        var query = new GetNewsByIdQuery()
        {
            NewsId = id
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}