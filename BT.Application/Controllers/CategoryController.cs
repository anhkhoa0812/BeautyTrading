using BT.Application.Common.Utils;
using BT.Application.Common.Validators;
using BT.Application.Features.Categories.Command.CreateCategory;
using BT.Domain.Constants;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Category.CategoryEndpoint)]
public class CategoryController : BaseController<CategoryController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<CreateCategoryCommand> _createCategoryValidationUtil;
    
    public CategoryController(IMediator mediator, ILogger logger,
        ValidationUtil<CreateCategoryCommand> createCategoryValidationUtil) : base(logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _createCategoryValidationUtil = createCategoryValidationUtil;
    }

    [CustomAuthorize(ERole.Admin)]
    [HttpPost(ApiEndPointConstant.Category.CategoryEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var (isValid, response) = await _createCategoryValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateCategory), apiResponse);
    }
}